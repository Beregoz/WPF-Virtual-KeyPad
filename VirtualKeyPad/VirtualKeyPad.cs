using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Control = System.Windows.Controls.Control;

namespace VirtualKeyPad
{
	public partial class VirtualKeyPad : Control
	{
		#region dll imports
		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
		#endregion

		#region fields
		private const int WmKeydown = 0x0100;
		private IntPtr _handle;

		private ICommand _keyPressCommand;
		#endregion

		#region properties
		public event EventHandler OkKeyClicked;
		public ICommand KeyPressCommand
		{
			get { return this._keyPressCommand ?? (this._keyPressCommand = new CommandBase(this.ExecuteKeyPress)); }
		}
		#endregion

		#region ctor
		static VirtualKeyPad()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(global::VirtualKeyPad.VirtualKeyPad),
				new FrameworkPropertyMetadata(typeof(global::VirtualKeyPad.VirtualKeyPad)));
		}
		
		public VirtualKeyPad()
		{
			this.Loaded += this.OnLoaded;
		}
		#endregion

		#region methods
		private void ExecuteKeyPress(object obj)
		{
			var key = (Key) obj;
			PostMessage(this._handle, WmKeydown, KeyInterop.VirtualKeyFromKey(key), 0);
			if (key == Key.Enter)
			{
				this.OnOkKeyClicked();
			}
		}

		public void SetParentVisual(Visual parent)
		{
			var source = (HwndSource) PresentationSource.FromVisual(parent);
			if (source != null)
			{
				this._handle = source.Handle;
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var source = (HwndSource) PresentationSource.FromVisual(this);
			if (source != null)
			{
				this._handle = source.Handle;
			}
		}

		protected virtual void OnOkKeyClicked()
		{
			var handler = this.OkKeyClicked;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
		#endregion
	}
}