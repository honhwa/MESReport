using System;
using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.Compatibility.VB6;

namespace Report.Library
{
    public class mdlEncryption
    {

        public static string EncryptPSW(string newPSW)
        {
            string EncryptPSW;
            string sTime;
            string UnPSWCode;
            string pswcode = "";
            sTime = Conversion.Str(DateAndTime.Timer);
            UnPSWCode = Strings.Right("00000" + sTime, 5) + newPSW + Strings.Right("00" + sTime, 2);
            Encrypt("UCtnd", UnPSWCode, ref pswcode);
            EncryptPSW = pswcode;
            return EncryptPSW;
        }


        public static string DecryptPSW(string pswcode)
        {
            string DecryptPSW;
            string TempDecrypt;
            int needLength;
            TempDecrypt = Decrypt("UCtnd", pswcode);
            needLength = TempDecrypt.Length - 7;
            DecryptPSW = Strings.Mid(TempDecrypt, 6, needLength);
            return DecryptPSW;
        }

        public static string Decrypt(string password, string from_text)
        {
            string Decrypt;
            const short MIN_ASC = 32; // Space.
            const short MAX_ASC = 126; // ~.
            const short NUM_ASC = MAX_ASC - MIN_ASC + 1;

            int offset;
            int str_len;
            int i;
            int ch;
            string to_text;


            offset = NumericPassword(password);
            VBMath.Rnd(-1);
            VBMath.Randomize(offset);
            to_text = "";

            str_len = from_text.Length;
            for (i = 1; i <= str_len; i++)
            {
                ch = Strings.Asc(from_text[i - 1].ToString());
                if (ch >= MIN_ASC && ch <= MAX_ASC)
                {
                    ch -= MIN_ASC;
                    //offset = Int((NUM_ASC + 1) * Rnd)
                    //20240116 penny: 轉double
                    double d = Convert.ToDouble((NUM_ASC + 1) * VBMath.Rnd());
                    //20240116 penny: floor向下取整, 再轉int32
                    offset = Convert.ToInt32(Math.Floor(d));
                    ch = ((ch - offset) % NUM_ASC);
                    if (ch < 0)
                    {
                        ch += NUM_ASC;
                    }
                    ch += MIN_ASC;
                    to_text += Convert.ToString(Convert.ToChar(ch));
                }
            } // i
            Decrypt = to_text;
            return Decrypt;
        }

        public static bool Encrypt(string password, string from_text, ref string to_text)
        {
            bool Encrypt = false;
            const short MIN_ASC = 32; // Space.
            const short MAX_ASC = 126; // ~.
            const short NUM_ASC = MAX_ASC - MIN_ASC + 1;

            int offset;
            int str_len;
            int i;
            int ch;

            offset = NumericPassword(password);
            VBMath.Rnd(-1);
            VBMath.Randomize(offset);
            str_len = from_text.Length;
            for (i = 1; i <= str_len; i++)
            {
                ch = Strings.Asc(from_text[i - 1].ToString());
                if (ch >= MIN_ASC && ch <= MAX_ASC)
                {
                    ch -= MIN_ASC;
                    //offset = Int((NUM_ASC + 1) * Rnd)
                    //20240116 penny: 轉double, 再floor向下取整, 再轉int32
                    double d = (NUM_ASC + 1) * VBMath.Rnd();
                    offset = Convert.ToInt32(Math.Floor(d));
                    ch = ((ch + offset) % NUM_ASC);
                    ch += MIN_ASC;
                    to_text += Convert.ToString(Convert.ToChar(ch));
                }
            } // i
            return Encrypt;
        }


        private static int NumericPassword(string password)
        {
            int NumericPassword;
            // VBto upgrade warning: mvalue As int	OnWrite(double)
            int mvalue = 0;
            int ch;
            int shift1 = 0;
            int shift2 = 0;
            int i;
            int str_len;

            str_len = password.Length;
            for (i = 1; i <= str_len; i++)
            {
                // Add the next letter.
                ch = Strings.Asc(password[i - 1].ToString());
                //mvalue = Convert.ToInt32(mvalue ^ (ch*Math.Pow(2, shift1)));
                //mvalue = Convert.ToInt32(mvalue ^ (ch*Math.Pow(2, shift2)));
                //mvalue = Convert.ToInt32(Math.Pow(mvalue, (ch * Math.Pow(2, shift1))));
                //mvalue = Convert.ToInt32(Math.Pow(mvalue, (ch * Math.Pow(2, shift2))));

                //20240115 penny: pow => 次方 , ^ => Xor
                mvalue = mvalue ^ Convert.ToInt32(ch * Math.Pow(2, shift1));
                mvalue = mvalue ^ Convert.ToInt32(ch * Math.Pow(2, shift2));

                //mvalue = mvalue Xor (ch * (2 ^ shift1))
                //mvalue = mvalue Xor (ch * (2 ^ shift2))

                // Change the shift offsets.
                shift1 = (shift1 + 7) % 19;
                shift2 = (shift2 + 13) % 23;
            } // i
            NumericPassword = mvalue;
            return NumericPassword;
        }
    }
}