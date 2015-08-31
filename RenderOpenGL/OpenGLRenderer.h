#pragma once

#include <windows.h>
#include <gl/gl.h>

namespace ThW
{
	namespace UI
	{
		namespace Utils
		{
			ref class OpenGLImage : public ThW::UI::Utils::IImage
			{
			public:
				OpenGLImage(int w, int h) :
					ThW::UI::Utils::IImage(),
					m_nTexture(new GLuint),
					m_bLoaded(false),
					w(w),
					h(h)
				{
				};

				OpenGLImage(int w, int h, unsigned char* pBytes) :
					ThW::UI::Utils::IImage(),
					m_nTexture(new GLuint),
					m_bLoaded(false),
					w(w),
					h(h)
				{
					LoadByteData(w, h, pBytes);
				};

				virtual ~OpenGLImage()
				{
					if (true == this->m_bLoaded)
					{
						::glDeleteTextures(1, this->m_nTexture);
					}

					delete this->m_nTexture;
				};

				GLuint GetTexture()
				{
					return *this->m_nTexture;
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
					bool get() override sealed { return this->m_bLoaded; };
				};

				virtual property bool LoadFailed
				{
					bool get() override sealed { return !this->m_bLoaded; };
				};

			private:
				void LoadByteData(int w, int h, unsigned char* pBytes)
				{
					::glGenTextures(1, this->m_nTexture);
					::glBindTexture(GL_TEXTURE_2D, *this->m_nTexture);
					::glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
					::glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
					::glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
					::glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
					::glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA8, w, h, 0, GL_RGBA, GL_UNSIGNED_BYTE, pBytes);
				};

			private:
				GLuint*	m_nTexture;
				bool	m_bLoaded;
				int		w;
				int		h;
			};

			public ref class OpenGLRender : public ThW::UI::Utils::IRender
			{
			public:
				OpenGLRender(intptr_t windowHandle) :
					m_hRC(NULL),
					m_hLibrary(NULL),
					m_hDC(NULL),
					m_hWnd(NULL),
					m_nWidth(800),
					m_nHeight(600)
				{
					HWND hWnd = (HWND)windowHandle;

					PIXELFORMATDESCRIPTOR pfd =
					{
						sizeof(PIXELFORMATDESCRIPTOR), 1, PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | /*PFD_GENERIC_ACCELERATED |*/ PFD_DOUBLEBUFFER, PFD_TYPE_RGBA,
						static_cast<BYTE>(32), // 32 color bits
						0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
						static_cast<BYTE>(24), // 24 Z-bits
						static_cast<BYTE>(1), // 0 - 1 stencil bit
						0, PFD_MAIN_PLANE, 0, 0, 0, 0
					};

					HDC hDC = ::GetDC(hWnd);

					if (hDC == NULL)
					{
						System::Console::WriteLine("error init OpenGL");
					}

					int iPixFormat = ::ChoosePixelFormat(hDC, &pfd);

					if (iPixFormat == 0)
					{
						::ReleaseDC(hWnd, hDC);
						System::Console::WriteLine("error init OpenGL");
					}

					if (!::DescribePixelFormat(hDC, iPixFormat, sizeof(PIXELFORMATDESCRIPTOR), &pfd))
					{
						::ReleaseDC(hWnd, hDC);
						System::Console::WriteLine("error init OpenGL");
					}

					int generic_format = pfd.dwFlags & PFD_GENERIC_FORMAT;
					int generic_accelerated = pfd.dwFlags & PFD_GENERIC_ACCELERATED;

					if (generic_format && !generic_accelerated)
					{
						::ReleaseDC(hWnd, hDC);
						System::Console::WriteLine("error init OpenGL");
					}
					else if (generic_format && generic_accelerated)
					{
						System::Console::WriteLine("Render: Hardware Acceleration found");
					}
					else if (!generic_format && !generic_accelerated)
					{
						System::Console::WriteLine("Render: Hardware Acceleration found");
					}

					if (!::SetPixelFormat(hDC, iPixFormat, &pfd))
					{
						::ReleaseDC(hWnd, hDC);
						System::Console::WriteLine("error init OpenGL");
					}

					if ((this->m_hRC = ::wglCreateContext(hDC)) == 0)
					{
						::ReleaseDC(hWnd, hDC);
						System::Console::WriteLine("error init OpenGL");
					}

					if (!::wglMakeCurrent(hDC, this->m_hRC))
					{
						::wglDeleteContext(this->m_hRC);
						::ReleaseDC(hWnd, hDC);
						System::Console::WriteLine("error init OpenGL");
					}

					this->m_hLibrary = ::LoadLibraryW(L"opengl32.dll");

					if (NULL == this->m_hLibrary)
					{
						::wglDeleteContext(this->m_hRC);
						::ReleaseDC(hWnd, hDC);
						System::Console::WriteLine("error init OpenGL");
					}

					this->m_hDC = hDC;
					this->m_hWnd = hWnd;

					::glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
					::glEnable(GL_TEXTURE_2D);
					::glEnableClientState(GL_VERTEX_ARRAY);
					::glEnableClientState(GL_TEXTURE_COORD_ARRAY);
					::glClearColor(1, 1, 0, 1);
				};

				virtual ~OpenGLRender()
				{
					if (NULL != this->m_hRC)
					{
						::wglDeleteContext(this->m_hRC);
						this->m_hRC = NULL;
					}

					if (NULL != this->m_hLibrary)
					{
						::FreeLibrary(this->m_hLibrary);
						this->m_hLibrary = NULL;
					}

					if (NULL != this->m_hDC)
					{
						::ReleaseDC(this->m_hWnd, this->m_hDC);
					}
				};

				virtual void SetBackColor(float r, float g, float b, float a) override
				{
					::glClearColor(r, g, b, a);
				};

				virtual void EndRender() override
				{
					::SwapBuffers(this->m_hDC);
				};

				virtual ThW::UI::Utils::IImage^ CreateImage(cli::array<unsigned char, 1>^, System::String^) override
				{
					return nullptr;
				};

				virtual void DrawImage(int x, int y, int w, int h, ThW::UI::Utils::IImage^ image, float us, float vs, float ue, float ve, ThW::UI::Utils::Color^ color, bool outLineOnly) override
				{
					::glColor4f(color->R, color->G, color->B, color->A);

					::glBindTexture(GL_TEXTURE_2D, static_cast<OpenGLImage^>(image)->GetTexture());

					float f = -0.5f;
					float vTexCoord[] = { us, vs, ue, vs, ue, ve, us, ve };
					::glTexCoordPointer(2, GL_FLOAT, 0, vTexCoord);

					if (true == outLineOnly)
					{
						float vCoordPointer[] = { x + f, y + f, x + w + f - 1.0f, y + f, x + w + f - 1.0f, y + h + f - 1.0f, x + f, y + h + f - 1.0f };

						::glVertexPointer(2, GL_FLOAT, 0, vCoordPointer);

						::glDisable(GL_TEXTURE_2D);
						::glDrawArrays(GL_LINE_LOOP, 0, 4);
						::glEnable(GL_TEXTURE_2D);
					}
					else
					{
						float vCoordPointer[] = { x + f, y + f, x + w + f, y + f, x + w + f, y + h + f, x + f, y + h + f };

						::glVertexPointer(2, GL_FLOAT, 0, vCoordPointer);

						::glDrawArrays(GL_TRIANGLE_FAN, 0, 4);
					}
				};

				virtual ThW::UI::Utils::IImage^ CreateImage(System::String^) override
				{
					return nullptr;
				};

				virtual ThW::UI::Utils::IImage^ CreateImage(int w, int h, cli::array<unsigned char, 1>^ bytes) override
				{
					pin_ptr<unsigned char> p = &bytes[0];

					return gcnew OpenGLImage(w, h, p);
				};

				virtual void DrawLine(int x1, int y1, int x2, int y2, ThW::UI::Utils::Color^ color) override
				{
					::glDisable(GL_TEXTURE_2D);
					::glColor4f(color->R, color->G, color->B, color->A);

					float f = 0.5f;

					float vTexCoord[] = { 0.0f, 0.0f, 0.1f, 0.1f };
					float vCoordPointer[] = { x1 + f, y1 + f, x2 + f, y2 + f };

					::glVertexPointer(2, GL_FLOAT, 0, vCoordPointer);
					::glTexCoordPointer(2, GL_FLOAT, 0, vTexCoord);
					::glDrawArrays(GL_LINE_LOOP, 0, 2);
					::glEnable(GL_TEXTURE_2D);
				};

				virtual void BeginRender()
				{
					::glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);
					::glMatrixMode(GL_PROJECTION);
					::glLoadIdentity();
					//::glOrtho(0.0, this->m_nWidth, this->m_nHeight, 0.0, 0.0, 1.0);

					float left = 0.0f;
					float right = static_cast<float>(this->m_nWidth);
					float top = 0.0f;
					float bottom = static_cast<float>(this->m_nHeight);
					float far1 = 1.0f;
					float near1 = 0.0f;
					float tx = -(right + left) / (right - left);
					float ty = -(top + bottom) / (top - bottom);
					float tz = -(far1 + near1) / (far1 - near1);

					float m[] =
					{
						2 / (right - left), 0, 0, 0,
						0, 2 / (top - bottom), 0, 0,
						0, 0, (-2) / (far1 - near1), 0,
						tx, ty, tz, 1
					};

					::glMultMatrixf(m);
					::glMatrixMode(GL_MODELVIEW);
					::glLoadIdentity();
					::glDisable(GL_DEPTH_TEST);
					::glDisable(GL_DITHER);
					::glDisable(GL_FOG);
					::glDisable(GL_LIGHTING);
					::glDisable(GL_LOGIC_OP);
					::glDisable(GL_STENCIL_TEST);
					::glEnable(GL_BLEND);
					::glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
					::glEnable(GL_TEXTURE_2D);
				};

				virtual void SetViewSize(int x, int y, int nWidth, int nHeight) override
				{
					this->m_nWidth = nWidth;
					this->m_nHeight = nHeight;

					::glViewport(x, -y, nWidth, nHeight);
				};

			private:
				HDC		m_hDC;
				HGLRC	m_hRC;
				HWND	m_hWnd;
				HMODULE	m_hLibrary;
				int		m_nWidth;
				int		m_nHeight;
			};
		};
	};
};
