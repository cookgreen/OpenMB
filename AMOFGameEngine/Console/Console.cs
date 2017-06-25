using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AMOFGameEngine.Console
{
    enum ConsoleItemType
    {
        CTYPE_UCHAR,
        CTYPE_CHAR,
        CTYPE_UINT,
        CTYPE_INT,
        CTYPE_FLOAT,
        CTYPE_STRING,
        CTYPE_FUNCTION
    }

    class ConsoleFunction : List<string>
    {

    }

    struct ConsoleItem
    {
        string name;
        ConsoleItemType type;

        [StructLayout(LayoutKind.Explicit)]
        struct Function
        {
            [FieldOffset(0)]
            ConsoleFunction function;
            [FieldOffset(0)]
            void variablePointer;
        }
    }

    class Console
    {
        public Console(int commandHistory, int textHistory, bool echo, ConsoleFunction defaultFunction, int lineIndex);
        public virtual ~Console();
 
        public void addItem(string strName, void pointer, ConsoleItemType type);
        public void deleteItem(string strName);
        public void print(string strText);
        public void setCommandBufferSize(int size);
        public int getCommandBufferSize() { return commandBufferSize; }
        public void clearCommandBuffer() { commandBuffer.Clear(); }
        public void setOutputBufferSize(int size);
        public int getOutputBufferSize() { return textBufferSize; }
        public void clearOutputBuffer() { textBuffer.Clear(); }
        public string getPrevCommand(int index);
        public void setDefaultCommand(ConsoleFunction def) { defaultCommand = def; }
        public void parseCommandQueue();
        public void sendMessage(string command);
        public void sendImmediateMessage(string command) { parseCommandLine(command); }
 
        public virtual void render();

        protected List<string> textBuffer;
        protected List<string> commandBuffer;
 
        private bool parseCommandLine(string commandLine);
 
        private bool commandEcho;
        private int lineIndex;
        private int commandBufferSize;
        private int textBufferSize;
        private ConsoleFunction defaultCommand;
        private ItemList itemList;
        private List<string> commandQueue;
    }
}
