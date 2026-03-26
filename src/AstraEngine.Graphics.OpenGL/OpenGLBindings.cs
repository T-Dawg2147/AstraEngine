using System.Runtime.InteropServices;
using AstraEngine.Platform.Windows;

namespace AstraEngine.Graphics.OpenGL;

internal sealed class OpenGLBindings
{
    private static readonly Lazy<OpenGLBindings> _instance = new(() => new OpenGLBindings());
    public static OpenGLBindings Instance => _instance.Value;

    // --- Existing delegates ---
    private delegate void GlClearColorDelegate(float red, float green, float blue, float alpha);
    private delegate void GlClearDelegate(uint mask);
    private delegate uint GlCreateShaderDelegate(uint type);
    private delegate void GlShaderSourceDelegate(uint shader, int count, string[]? source, int[]? length);
    private delegate void GlCompileShaderDelegate(uint shader);
    private delegate void GlGetShaderivDelegate(uint shader, uint pname, out int param);
    private delegate void GlGetShaderInfoLogDelegate(uint shader, int bufSize, out int length, byte[] infoLog);
    private delegate uint GlCreateProgramDelegate();
    private delegate void GlAttachShaderDelegate(uint program, uint shader);
    private delegate void GlLinkProgramDelegate(uint program);
    private delegate void GlGetProgramivDelegate(uint program, uint pname, out int param);
    private delegate void GlGetProgramInfoLogDelegate(uint program, int bufSize, out int length, byte[] infoLog);
    private delegate void GlUseProgramDelegate(uint program);
    private delegate void GlDeleteShaderDelegate(uint shader);
    private delegate void GlGenVertexArraysDelegate(int n, out uint arrays);
    private delegate void GlBindVertexArrayDelegate(uint array);
    private delegate void GlGenBuffersDelegate(int n, out uint buffers);
    private delegate void GlBindBufferDelegate(uint target, uint buffer);
    private delegate void GlBufferDataDelegate(uint target, nuint size, nint data, uint usage);
    private delegate void GlEnableVertexAttribArrayDelegate(uint index);
    private delegate void GlVertexAttribPointerDelegate(uint index, int size, uint type, bool normalized, int stride, nint pointer);
    private delegate void GlDrawArraysDelegate(uint mode, int first, int count);
    private delegate void GlViewportDelegate(int x, int y, int width, int height);
    private delegate void GlEnableDelegate(uint cap);
    private delegate void GlDepthFuncDelegate(uint func);
    private delegate void GlCullFaceDelegate(uint mode);

    // --- New delegates ---
    private delegate int GlGetUniformLocationDelegate(uint program, string name);
    private delegate void GlUniformMatrix4fvDelegate(int location, int count, bool transpose, nint value);
    private delegate void GlUniform1fDelegate(int location, float v0);
    private delegate void GlUniform1iDelegate(int location, int v0);
    private delegate void GlUniform3fDelegate(int location, float v0, float v1, float v2);
    private delegate void GlUniform4fDelegate(int location, float v0, float v1, float v2, float v3);
    private delegate void GlDrawElementsDelegate(uint mode, int count, uint type, nint indices);
    private delegate void GlFrontFaceDelegate(uint mode);
    private delegate void GlDeleteProgramDelegate(uint program);
    private delegate void GlDeleteVertexArraysDelegate(int n, ref uint arrays);
    private delegate void GlDeleteBuffersDelegate(int n, ref uint buffers);

    // --- Existing fields ---
    private readonly GlClearColorDelegate _clearColor;
    private readonly GlClearDelegate _clear;
    private readonly GlCreateShaderDelegate _createShader;
    private readonly GlShaderSourceDelegate _shaderSource;
    private readonly GlCompileShaderDelegate _compileShader;
    private readonly GlGetShaderivDelegate _getShaderiv;
    private readonly GlGetShaderInfoLogDelegate _getShaderInfoLog;
    private readonly GlCreateProgramDelegate _createProgram;
    private readonly GlAttachShaderDelegate _attachShader;
    private readonly GlLinkProgramDelegate _linkProgram;
    private readonly GlGetProgramivDelegate _getProgramiv;
    private readonly GlGetProgramInfoLogDelegate _getProgramInfoLog;
    private readonly GlUseProgramDelegate _useProgram;
    private readonly GlDeleteShaderDelegate _deleteShader;
    private readonly GlGenVertexArraysDelegate _genVertexArrays;
    private readonly GlBindVertexArrayDelegate _bindVertexArray;
    private readonly GlGenBuffersDelegate _genBuffers;
    private readonly GlBindBufferDelegate _bindBuffer;
    private readonly GlBufferDataDelegate _bufferData;
    private readonly GlEnableVertexAttribArrayDelegate _enableVertexAttribArray;
    private readonly GlVertexAttribPointerDelegate _vertexAttribPointer;
    private readonly GlDrawArraysDelegate _drawArrays;
    private readonly GlViewportDelegate _viewport;
    private readonly GlEnableDelegate _enable;
    private readonly GlDepthFuncDelegate _depthFunc;
    private readonly GlCullFaceDelegate _cullFace;

    // --- New fields ---
    private readonly GlGetUniformLocationDelegate _getUniformLocation;
    private readonly GlUniformMatrix4fvDelegate _uniformMatrix4fv;
    private readonly GlUniform1fDelegate _uniform1f;
    private readonly GlUniform1iDelegate _uniform1i;
    private readonly GlUniform3fDelegate _uniform3f;
    private readonly GlUniform4fDelegate _uniform4f;
    private readonly GlDrawElementsDelegate _drawElements;
    private readonly GlFrontFaceDelegate _frontFace;
    private readonly GlDeleteProgramDelegate _deleteProgram;
    private readonly GlDeleteVertexArraysDelegate _deleteVertexArrays;
    private readonly GlDeleteBuffersDelegate _deleteBuffers;

    private OpenGLBindings()
    {
        // --- Existing loads ---
        _clearColor = Load<GlClearColorDelegate>("glClearColor");
        _clear = Load<GlClearDelegate>("glClear");
        _createShader = Load<GlCreateShaderDelegate>("glCreateShader");
        _shaderSource = Load<GlShaderSourceDelegate>("glShaderSource");
        _compileShader = Load<GlCompileShaderDelegate>("glCompileShader");
        _getShaderiv = Load<GlGetShaderivDelegate>("glGetShaderiv");
        _getShaderInfoLog = Load<GlGetShaderInfoLogDelegate>("glGetShaderInfoLog");
        _createProgram = Load<GlCreateProgramDelegate>("glCreateProgram");
        _attachShader = Load<GlAttachShaderDelegate>("glAttachShader");
        _linkProgram = Load<GlLinkProgramDelegate>("glLinkProgram");
        _getProgramiv = Load<GlGetProgramivDelegate>("glGetProgramiv");
        _getProgramInfoLog = Load<GlGetProgramInfoLogDelegate>("glGetProgramInfoLog");
        _useProgram = Load<GlUseProgramDelegate>("glUseProgram");
        _deleteShader = Load<GlDeleteShaderDelegate>("glDeleteShader");
        _genVertexArrays = Load<GlGenVertexArraysDelegate>("glGenVertexArrays");
        _bindVertexArray = Load<GlBindVertexArrayDelegate>("glBindVertexArray");
        _genBuffers = Load<GlGenBuffersDelegate>("glGenBuffers");
        _bindBuffer = Load<GlBindBufferDelegate>("glBindBuffer");
        _bufferData = Load<GlBufferDataDelegate>("glBufferData");
        _enableVertexAttribArray = Load<GlEnableVertexAttribArrayDelegate>("glEnableVertexAttribArray");
        _vertexAttribPointer = Load<GlVertexAttribPointerDelegate>("glVertexAttribPointer");
        _drawArrays = Load<GlDrawArraysDelegate>("glDrawArrays");
        _viewport = Load<GlViewportDelegate>("glViewport");
        _enable = Load<GlEnableDelegate>("glEnable");
        _depthFunc = Load<GlDepthFuncDelegate>("glDepthFunc");
        _cullFace = Load<GlCullFaceDelegate>("glCullFace");

        // --- New loads ---
        _getUniformLocation = Load<GlGetUniformLocationDelegate>("glGetUniformLocation");
        _uniformMatrix4fv = Load<GlUniformMatrix4fvDelegate>("glUniformMatrix4fv");
        _uniform1f = Load<GlUniform1fDelegate>("glUniform1f");
        _uniform1i = Load<GlUniform1iDelegate>("glUniform1i");
        _uniform3f = Load<GlUniform3fDelegate>("glUniform3f");
        _uniform4f = Load<GlUniform4fDelegate>("glUniform4f");
        _drawElements = Load<GlDrawElementsDelegate>("glDrawElements");
        _frontFace = Load<GlFrontFaceDelegate>("glFrontFace");
        _deleteProgram = Load<GlDeleteProgramDelegate>("glDeleteProgram");
        _deleteVertexArrays = Load<GlDeleteVertexArraysDelegate>("glDeleteVertexArrays");
        _deleteBuffers = Load<GlDeleteBuffersDelegate>("glDeleteBuffers");
    }

    // --- Existing public methods ---
    public void ClearColor(float r, float g, float b, float a) => _clearColor(r, g, b, a);
    public void Clear(uint mask) => _clear(mask);
    public uint CreateShader(uint type) => _createShader(type);
    public void ShaderSource(uint shader, string source) => _shaderSource(shader, 1, [source], null);
    public void CompileShader(uint shader) => _compileShader(shader);
    public void GetShaderiv(uint shader, uint pname, out int param) => _getShaderiv(shader, pname, out param);
    public string GetShaderInfoLog(uint shader)
    {
        var buffer = new byte[4096];
        _getShaderInfoLog(shader, buffer.Length, out var length, buffer);
        return System.Text.Encoding.UTF8.GetString(buffer, 0, length);
    }
    public uint CreateProgram() => _createProgram();
    public void AttachShader(uint program, uint shader) => _attachShader(program, shader);
    public void LinkProgram(uint program) => _linkProgram(program);
    public void GetProgramiv(uint program, uint pname, out int param) => _getProgramiv(program, pname, out param);
    public string GetProgramInfoLog(uint program)
    {
        var buffer = new byte[4096];
        _getProgramInfoLog(program, buffer.Length, out var length, buffer);
        return System.Text.Encoding.UTF8.GetString(buffer, 0, length);
    }
    public void UseProgram(uint program) => _useProgram(program);
    public void DeleteShader(uint shader) => _deleteShader(shader);
    public void GenVertexArrays(int n, out uint arrays) => _genVertexArrays(n, out arrays);
    public void BindVertexArray(uint array) => _bindVertexArray(array);
    public void GenBuffers(int n, out uint buffers) => _genBuffers(n, out buffers);
    public void BindBuffer(uint target, uint buffer) => _bindBuffer(target, buffer);
    public void BufferData(uint target, nuint size, nint data, uint usage) => _bufferData(target, size, data, usage);
    public void EnableVertexAttribArray(uint index) => _enableVertexAttribArray(index);
    public void VertexAttribPointer(uint index, int size, uint type, bool normalized, int stride, nint pointer) => _vertexAttribPointer(index, size, type, normalized, stride, pointer);
    public void DrawArrays(uint mode, int first, int count) => _drawArrays(mode, first, count);
    public void Viewport(int x, int y, int width, int height) => _viewport(x, y, width, height);
    public void Enable(uint cap) => _enable(cap);
    public void DepthFunc(uint func) => _depthFunc(func);
    public void CullFace(uint mode) => _cullFace(mode);

    // --- New public methods ---
    public int GetUniformLocation(uint program, string name) => _getUniformLocation(program, name);

    public void UniformMatrix4fv(int location, int count, bool transpose, nint value)
        => _uniformMatrix4fv(location, count, transpose, value);

    public void Uniform1f(int location, float v0) => _uniform1f(location, v0);
    public void Uniform1i(int location, int v0) => _uniform1i(location, v0);
    public void Uniform3f(int location, float v0, float v1, float v2) => _uniform3f(location, v0, v1, v2);
    public void Uniform4f(int location, float v0, float v1, float v2, float v3) => _uniform4f(location, v0, v1, v2, v3);

    public void DrawElements(uint mode, int count, uint type, nint indices)
        => _drawElements(mode, count, type, indices);

    public void FrontFace(uint mode) => _frontFace(mode);
    public void DeleteProgram(uint program) => _deleteProgram(program);
    public void DeleteVertexArrays(int n, ref uint arrays) => _deleteVertexArrays(n, ref arrays);
    public void DeleteBuffers(int n, ref uint buffers) => _deleteBuffers(n, ref buffers);

    private static T Load<T>(string name) where T : Delegate
    {
        var proc = OpenGLProcLoader.Load(name);
        if (proc == 0)
        {
            throw new InvalidOperationException($"Failed to load OpenGL function: {name}");
        }

        return Marshal.GetDelegateForFunctionPointer<T>(proc);
    }
}