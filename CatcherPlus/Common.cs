using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Common
{

				public class Cmt
				{
												
								public string name;
								public string date;
								public string location;
								public string content;
								public string up;
				}

				public class CatcherWorker<T>
				{
								private T obj;

								public CatcherWorker(T obj)
								{
												this.obj = obj;
								}
								public void Run()
								{
												Type t = obj.GetType();
												MethodInfo mrun = t.GetMethod("Run");
												mrun.Invoke(obj, null);
								}
				}
}
