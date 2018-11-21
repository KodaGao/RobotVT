namespace SK_FModel
{
    public class SystemDelegate
    {
        public delegate void del_RuningMessage(object sender, string MessageInfo, SystemEnum.MessageType Type, params object[] paras);

        public delegate void del_DebugMessage(object sender, object Message);

        public delegate void del_InitSystem(string Message);

        public delegate void del_SystemLoadFinish();

        public delegate void del_SystemSetFinish();
        
        public delegate void PlayView_SystemMouseDoubleClick(string vtID);
    }
}