﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClassLibrary.ProcessingsClient
{
	public abstract class ProcessingClient
	{
		public virtual void Process(ControllerClient Model)
		{
			MessageBox.Show("Как так");
		}
	}
}
