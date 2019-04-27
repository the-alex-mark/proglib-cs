using System;

namespace radio42.Multimedia.Midi
{
	public enum MIDIMessage
	{
		MIM_OPEN = 961,
		MIM_CLOSE,
		MIM_DATA,
		MIM_LONGDATA,
		MIM_ERROR,
		MIM_LONGERROR,
		MIM_MOREDATA = 972,
		MOM_OPEN = 967,
		MOM_CLOSE,
		MOM_DONE
	}
}
