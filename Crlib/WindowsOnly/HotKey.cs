using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace REVUnit.Crlib.WindowsOnly
{
    public sealed class HotKey : IMessageFilter
    {
        [Flags]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        public HotKey(IntPtr handle, int id, KeyModifiers modifiers, Keys key)
        {
            if (key == Keys.None || modifiers == KeyModifiers.None)
                throw new ArgumentException("The key or modifiers could not be None.");
            Handle = handle;
            Id = id;
            Modifiers = modifiers;
            Key = key;
            Register();
            Application.AddMessageFilter(this);
        }

        public IntPtr Handle { get; }

        public int Id { get; }

        public KeyModifiers Modifiers { get; }

        public Keys Key { get; }

        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == Native.WmHotkey && m.HWnd == Handle && m.WParam == (IntPtr) Id && HotKeyPressed != null)
            {
                HotKeyPressed(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        public static KeyModifiers GetModifiersFromKeys(Keys keydata, out Keys key)
        {
            key = keydata;
            var keyModifiers = KeyModifiers.None;
            if ((keydata & Keys.Control) == Keys.Control)
            {
                keyModifiers |= KeyModifiers.Control;
                key = keydata ^ Keys.Control;
            }

            if ((keydata & Keys.Shift) == Keys.Shift)
            {
                keyModifiers |= KeyModifiers.Shift;
                key ^= Keys.Shift;
            }

            if ((keydata & Keys.Alt) == Keys.Alt)
            {
                keyModifiers |= KeyModifiers.Alt;
                key ^= Keys.Alt;
            }

            if ((keydata & Keys.LWin) == Keys.LWin)
            {
                keyModifiers |= KeyModifiers.Windows;
                key ^= Keys.LWin;
            }

            if ((keydata & Keys.RWin) == Keys.RWin)
            {
                keyModifiers |= KeyModifiers.Windows;
                key ^= Keys.RWin;
            }

            if (key == Keys.ShiftKey || key == Keys.ControlKey || key == Keys.Menu || key == Keys.LWin ||
                key == Keys.RWin)
                key = Keys.None;
            return keyModifiers;
        }

        public event EventHandler HotKeyPressed;

        private void Register()
        {
            if (!Native.RegisterHotKey(Handle, Id, Modifiers, Key))
            {
                Native.UnregisterHotKey(IntPtr.Zero, Id);
                if (!Native.RegisterHotKey(Handle, Id, Modifiers, Key))
                    throw new ApplicationException("The hotkey is in use");
            }
        }

        public void Unregister()
        {
            Application.RemoveMessageFilter(this);
            Native.UnregisterHotKey(Handle, Id);
        }
    }
}