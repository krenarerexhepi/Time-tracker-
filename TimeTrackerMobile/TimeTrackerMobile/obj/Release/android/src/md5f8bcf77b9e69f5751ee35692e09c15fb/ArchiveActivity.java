package md5f8bcf77b9e69f5751ee35692e09c15fb;


public class ArchiveActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("TimeTrackerMobile.ArchiveActivity, TimeTrackerMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ArchiveActivity.class, __md_methods);
	}


	public ArchiveActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ArchiveActivity.class)
			mono.android.TypeManager.Activate ("TimeTrackerMobile.ArchiveActivity, TimeTrackerMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
