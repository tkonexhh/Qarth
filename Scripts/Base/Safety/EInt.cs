//  Desc:        Framework For Game Develop with Unity3d
//  Copyright:   Copyright (C) 2017 SnowCold. All rights reserved.
//  WebSite:     https://github.com/SnowCold/Qarth
//  Blog:        http://blog.csdn.net/snowcoldgame
//  Author:      SnowCold
//  E-mail:      snowcold.ouyang@gmail.com
#if UNITY_EDITOR
#define DEBUG_EDATA
#endif
//#define DEBUG_EDATA

public class EIntConst
{
    public static readonly int DATA_KEY = 0x2dF8a235;
    public static readonly int CRC_KEY = 0x738d40f9;
}

public struct EInt
{
    public int m_d;
    public int m_c;

#if DEBUG_EDATA
    //private int m_debugVal;
#endif

    public EInt(int value)
    {
		m_d = 0 ^ EIntConst.DATA_KEY;
		m_c = m_d ^ EIntConst.CRC_KEY;
#if DEBUG_EDATA
        //m_debugVal = 0;
#endif
        SetValue(value);
    }

    public void SetValue(int val)
    {
        m_d = val ^ EIntConst.DATA_KEY;
        m_c = m_d ^ EIntConst.CRC_KEY;
#if DEBUG_EDATA
        //m_debugVal = val;
#endif
    }

    public int GetValue()
    {
        // NOTE: 此处过滤了结构没有被调用构造方法而出现的校验计算未执行的情况(此时校验检查不成立)
        // IMPORTANT: 此过滤导致了漏过内存复位为0的情况
        if ((m_d | m_c) == 0)
        {
            return 0;
        }

        if ((m_d ^ EIntConst.CRC_KEY) != m_c)
        {
            //CheckMgr.S.CheaterFound();
            return 0;
        }

        return m_d ^ EIntConst.DATA_KEY;
    }

	public override string ToString ()
	{
		return this.GetValue().ToString();
	}

    public static implicit operator int(EInt value)
    {
        return value.GetValue();
    }

    public static implicit operator EInt(int value)
    {
        return (new EInt(value));
    }
}
