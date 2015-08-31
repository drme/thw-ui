#pragma once

#include <windowsx.h>
#include <d3dx9.h>
#include <d3d9.h>
#include <string>

#define D3DFVF_CUSTOMVERTEX (D3DFVF_XYZRHW | D3DFVF_DIFFUSE | D3DFVF_TEX1)

namespace ThW
{
	namespace UI
	{
		namespace Utils
		{
			private struct CUSTOMVERTEX
			{
				float x, y, z, rhw;
				D3DCOLOR color;
				float tu, tv;
			};

			ref class D3DImage : public ThW::UI::Utils::IImage
			{
				public:
					D3DImage(LPDIRECT3DDEVICE9 pD3dDevice, int w, int h, unsigned char* pBytes) :
						IImage(),
						m_pTexture(new LPDIRECT3DTEXTURE9),
						m_pD3dDevice(pD3dDevice)
					{
						LoadByteData(w, h, pBytes);
					};

					D3DImage(LPDIRECT3DDEVICE9 pD3dDevice, System::String^ strFileName1) :
						IImage(),
						m_pTexture(new LPDIRECT3DTEXTURE9),
						m_pD3dDevice(pD3dDevice)
					{
						cli::array<wchar_t,1>^ chars = strFileName1->ToCharArray();

						pin_ptr<wchar_t> pp = &chars[0];
						
						std::wstring strFileName = pp;

						if (FAILED(::D3DXCreateTextureFromFile(pD3dDevice, strFileName.c_str(), this->m_pTexture)))
						{
							if (FAILED(::D3DXCreateTextureFromFile(pD3dDevice, (strFileName + L".png").c_str(), this->m_pTexture)))
							{
								if (FAILED(::D3DXCreateTextureFromFile(pD3dDevice, (strFileName + L".tga").c_str(), this->m_pTexture)))
								{
									if (FAILED(::D3DXCreateTextureFromFile(pD3dDevice, (strFileName + L".jpg").c_str(), this->m_pTexture)))
									{
										this->SetSize(0, 0);
										return;
									}
								}
							}
						}

						D3DSURFACE_DESC d;
						(*this->m_pTexture)->GetLevelDesc(0, &d);
						this->SetSize(d.Width, d.Height);
					};

					D3DImage(LPDIRECT3DDEVICE9 pD3dDevice, unsigned char* pFileBytes, int nSize) :
						IImage(),
						m_pTexture(new LPDIRECT3DTEXTURE9),
						m_pD3dDevice(pD3dDevice)
					{
						if (FAILED(::D3DXCreateTextureFromFileInMemory(this->m_pD3dDevice, pFileBytes, nSize, this->m_pTexture)))
						{
							this->SetSize(0, 0);
						}
						else
						{
							D3DSURFACE_DESC d;

							(*this->m_pTexture)->GetLevelDesc(0, &d);
							this->SetSize(d.Width, d.Height);
						}
					};

					virtual ~D3DImage()
					{
						if (NULL != (*this->m_pTexture))
						{
							(*this->m_pTexture)->Release();
						}

						delete this->m_pTexture;
					};

					LPDIRECT3DTEXTURE9 GetTexture()
					{
						return (*this->m_pTexture);
					};

					virtual property int Width
					{
						int get() override sealed { return this->w; };
					};

					virtual property int Height
					{
						int get() override sealed { return this->h; };
					};

					virtual property bool Loaded
					{
						bool get() override sealed { return this->w > 0; };
					};

					virtual property bool LoadFailed
					{
						bool get() override sealed { return this->w <= 0; };
					};

				private:
					void LoadByteData(int w, int h, unsigned char* pBytes)
					{
						HRESULT hr = this->m_pD3dDevice->CreateTexture(w, h, 1, D3DUSAGE_DYNAMIC, D3DFMT_A8R8G8B8, D3DPOOL_DEFAULT, this->m_pTexture, NULL);

						if (D3DERR_INVALIDCALL == hr)
						{
							throw gcnew System::Exception("D3D texture init fail");
						}

						if (FAILED(hr))
						{
							throw gcnew System::Exception("D3D texture init fail");
						}

						D3DLOCKED_RECT r;

						::ZeroMemory(&r, sizeof(D3DLOCKED_RECT));

						hr = (*this->m_pTexture)->LockRect(0, &r, NULL, 0);

						unsigned char* pBuf = static_cast<unsigned char*>(r.pBits);

						for (int j = 0; j < h; j++)
						{
							for (int i = 0; i < w; i++)
							{
								pBuf[i * 4 + j * r.Pitch + 0] = pBytes[j * w * 4 + i * 4 + 2]; //B
								pBuf[i * 4 + j * r.Pitch + 1] = pBytes[j * w * 4 + i * 4 + 1]; //G
								pBuf[i * 4 + j * r.Pitch + 2] = pBytes[j * w * 4 + i * 4 + 0]; //R
								pBuf[i * 4 + j * r.Pitch + 3] = pBytes[j * w * 4 + i * 4 + 3]; //A
							}
						}
			
						hr = (*this->m_pTexture)->UnlockRect(0);

						D3DSURFACE_DESC d;

						(*this->m_pTexture)->GetLevelDesc(0, &d);

						this->SetSize(d.Width, d.Height);
					};

					private:
						void SetSize(int w, int h)
						{
							this->w = w;
							this->h = h;
						}

						int w;
						int h;

				private:
					LPDIRECT3DTEXTURE9* m_pTexture;
					LPDIRECT3DDEVICE9 m_pD3dDevice;
			};

			public ref class D3DRender : public ThW::UI::Utils::IRender
			{
				public:
					D3DRender(intptr_t windowHandle):
						m_pD3dDevice(new LPDIRECT3DDEVICE9),
						m_pD3D(NULL),
						m_hWnd((HWND)windowHandle),
						m_color(D3DCOLOR_XRGB(0, 0, 0xff))
					{
						HWND hWnd = (HWND)windowHandle;

						if (NULL == (this->m_pD3D = ::Direct3DCreate9(D3D_SDK_VERSION)))
						{
							throw gcnew System::Exception(L"D3D init failed");
						}

						D3DPRESENT_PARAMETERS d3dpp;

						::ZeroMemory(&d3dpp, sizeof(d3dpp));

						d3dpp.Windowed = TRUE;
						d3dpp.SwapEffect = D3DSWAPEFFECT_DISCARD;
						d3dpp.BackBufferFormat = D3DFMT_UNKNOWN;
						d3dpp.PresentationInterval = D3DPRESENT_DONOTWAIT;
						d3dpp.BackBufferCount = 1;
						d3dpp.hDeviceWindow = hWnd;

						if (FAILED(this->m_pD3D->CreateDevice(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, hWnd, D3DCREATE_SOFTWARE_VERTEXPROCESSING, &d3dpp, this->m_pD3dDevice)))
						{
							throw gcnew System::Exception(L"D3D init failed");
						}

						(*this->m_pD3dDevice)->SetRenderState(D3DRS_ALPHABLENDENABLE, true);
						(*this->m_pD3dDevice)->SetRenderState(D3DRS_SRCBLEND, D3DBLEND_SRCALPHA);
						(*this->m_pD3dDevice)->SetRenderState(D3DRS_DESTBLEND, D3DBLEND_INVSRCALPHA);
						//this->m_pD3dDevice->SetTextureStageState(0, D3DTSS_ALPHAARG1, D3DTA_TEXTURE);
						(*this->m_pD3dDevice)->SetTextureStageState(0, D3DTSS_ALPHAARG2, D3DTA_DIFFUSE);
						(*this->m_pD3dDevice)->SetTextureStageState(0, D3DTSS_ALPHAARG1, D3DTA_TEXTURE);
						(*this->m_pD3dDevice)->SetTextureStageState(0, D3DTSS_ALPHAOP, D3DTOP_MODULATE);
						//this->m_pD3dDevice->SetRenderState(D3DRS_STENCILENABLE, FALSE);
					};

					virtual ~D3DRender()
					{
						if ((*this->m_pD3dDevice) != NULL)
						{
							(*this->m_pD3dDevice)->Release();
						}

						if (this->m_pD3D != NULL)
						{
							this->m_pD3D->Release();
							this->m_pD3D = NULL;
						}

						delete this->m_pD3dDevice;
					};

					virtual void SetBackColor(float r, float g, float b, float a)
					{
						this->m_color = D3DCOLOR_XRGB((int)(r*0xff), (int)(g*0xff), (int)(b*0xff));
					};

					virtual void EndRender()
					{
						(*this->m_pD3dDevice)->EndScene();

						if (D3DERR_DEVICELOST == (*this->m_pD3dDevice)->Present(NULL, NULL, NULL, NULL))
						{
						}
						else
						{
						}
					};

					virtual ThW::UI::Utils::IImage^ CreateImage(cli::array<unsigned char,1>^ pFileBytes, System::String^ fileName) override
					{
						pin_ptr<unsigned char> bytes = &pFileBytes[0];

						D3DImage^ pImage = gcnew D3DImage(*this->m_pD3dDevice, bytes, pFileBytes->Length);

						if (0 == pImage->Width)
						{
							return nullptr;
						}
						else
						{
							return pImage;
						}
					};

					virtual void DrawImage(int x, int y, int w, int h, ThW::UI::Utils::IImage^ image, float us, float vs, float ue, float ve, ThW::UI::Utils::Color^ color, bool outLineOnly) override
					{
						const static float nMagic = -0.5f;

						CUSTOMVERTEX v[4];

						::ZeroMemory(v, 4 * sizeof(CUSTOMVERTEX));

						v[0].color = D3DCOLOR_COLORVALUE(color->R, color->G, color->B, color->A);
						v[0].rhw = 1.0f;
						v[0].x = static_cast<float>(x) + nMagic;
						v[0].y = static_cast<float>(y) + nMagic;
						v[0].z = nMagic;
						v[0].tu = us;
						v[0].tv = vs;

						v[1].color = v[0].color;
						v[1].rhw = 1.0f;
						v[1].x = static_cast<float>(x + w) + nMagic;
						v[1].y = static_cast<float>(y) + nMagic;
						v[1].z = nMagic;
						v[1].tu = ue;
						v[1].tv = vs;

						v[2].color = v[0].color;
						v[2].rhw = 1.0f;
						v[2].x = static_cast<float>(x + w) + nMagic;
						v[2].y = static_cast<float>(y + h) + nMagic;
						v[2].z = nMagic;
						v[2].tu = ue;
						v[2].tv = ve;

						v[3].color = v[0].color;
						v[3].rhw = 1.0f;
						v[3].x = static_cast<float>(x) + nMagic;
						v[3].y = static_cast<float>(y + h) + nMagic;
						v[3].z = nMagic;
						v[3].tu = us;
						v[3].tv = ve;

						(*this->m_pD3dDevice)->SetTexture(0, static_cast<D3DImage^>(image)->GetTexture());
						(*this->m_pD3dDevice)->SetFVF(D3DFVF_CUSTOMVERTEX);
						
						if (true == outLineOnly)
						{
							(*this->m_pD3dDevice)->DrawPrimitiveUP(D3DPT_LINESTRIP , 2, v, sizeof(CUSTOMVERTEX));
						}
						else
						{
							(*this->m_pD3dDevice)->DrawPrimitiveUP(D3DPT_TRIANGLEFAN , 2, v, sizeof(CUSTOMVERTEX));
						}
					};

					virtual ThW::UI::Utils::IImage^ CreateImage(System::String^ strFileName) override
					{
						D3DImage^ pImage = gcnew D3DImage(*this->m_pD3dDevice, strFileName);

						if (0 == pImage->Width)
						{
							return nullptr;
						}
						else
						{
							return pImage;
						}
					};

					virtual ThW::UI::Utils::IImage^ CreateImage(int w, int h, cli::array<unsigned char,1>^ bytes) override
					{
						pin_ptr<unsigned char> p = &bytes[0];

						return gcnew D3DImage(*this->m_pD3dDevice, w, h, p);
					};

					virtual void DrawLine(int x1, int y1, int x2, int y2, ThW::UI::Utils::Color^ color) override
					{
						const static float nMagic = -0.5f;

						CUSTOMVERTEX v[2];

						v[0].color = D3DCOLOR_COLORVALUE(color->R, color->G, color->B, color->A);
						v[0].rhw = 1.0f;
						v[0].x = static_cast<float>(x1) + nMagic;
						v[0].y = static_cast<float>(y1+1) + nMagic;
						v[0].z = nMagic;
						v[0].tv = 0.0f;
						v[0].tu = 0.0f;

						v[1].color = v[0].color;
						v[1].rhw = 1.0f;
						v[1].x = static_cast<float>(x2) + nMagic;
						v[1].y = static_cast<float>(y2+1) + nMagic;
						v[1].z = nMagic;
						v[1].tv = 1.0f;
						v[1].tu = 1.0f;

						(*this->m_pD3dDevice)->SetTexture(0, NULL);
 						(*this->m_pD3dDevice)->SetFVF(D3DFVF_CUSTOMVERTEX);
						(*this->m_pD3dDevice)->DrawPrimitiveUP(D3DPT_LINELIST, 1, v, sizeof(CUSTOMVERTEX));
					};

					virtual void BeginRender()
					{
						(*this->m_pD3dDevice)->Clear(0, NULL, D3DCLEAR_TARGET, this->m_color, 1.0f, 0);

						if (SUCCEEDED((*this->m_pD3dDevice)->BeginScene()))
						{
						}
						else
						{
						}
					};

					virtual void SetViewSize(int x, int y, int nWidth, int nHeight) override
					{
						D3DPRESENT_PARAMETERS d3dpp;

						::ZeroMemory(&d3dpp, sizeof(d3dpp));

						d3dpp.Windowed = TRUE;
						d3dpp.SwapEffect = D3DSWAPEFFECT_DISCARD;
						d3dpp.BackBufferFormat = D3DFMT_UNKNOWN;
						d3dpp.PresentationInterval = D3DPRESENT_DONOTWAIT;
						d3dpp.BackBufferCount = 1;
						d3dpp.hDeviceWindow = this->m_hWnd;
						d3dpp.BackBufferWidth = nWidth;
						d3dpp.BackBufferHeight = nHeight;

						//(*this->m_pD3dDevice)->Reset(&d3dpp);
					};

				private:
					HWND				m_hWnd;
					D3DCOLOR			m_color;
					LPDIRECT3DDEVICE9*	m_pD3dDevice;
					LPDIRECT3D9			m_pD3D;
			};
		};
	};
};
