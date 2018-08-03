namespace SK_FModel
{
    public class SystemDelegate
    {
        public delegate void del_InitSystem(string Message);

        public delegate void del_SystemLoadFinish();

        public delegate void del_SystemSetFinish();
        
        public delegate void PlayView_SystemMouseDoubleClick(string vtID);

    }
}