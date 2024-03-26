using UnityEngine;

namespace I2.Loc
{
	public static class I2Localize
	{

		public static string Don_t_Have_an_account_ 		{ get{ return LocalizationManager.GetTranslation ("Don t Have an account?"); } }
		public static string Email 		{ get{ return LocalizationManager.GetTranslation ("Email"); } }
		public static string Forgot_passwod 		{ get{ return LocalizationManager.GetTranslation ("Forgot passwod"); } }
		public static string Log_In 		{ get{ return LocalizationManager.GetTranslation ("Log In"); } }
		public static string Password 		{ get{ return LocalizationManager.GetTranslation ("Password"); } }
		public static string Remembe_me 		{ get{ return LocalizationManager.GetTranslation ("Remembe me"); } }
		public static string Sign_up 		{ get{ return LocalizationManager.GetTranslation ("Sign up"); } }
		public static string 语言 		{ get{ return LocalizationManager.GetTranslation ("语言"); } }
	}

    public static class I2Terms
	{

		public const string Don_t_Have_an_account_ = "Don t Have an account?";
		public const string Email = "Email";
		public const string Forgot_passwod = "Forgot passwod";
		public const string Log_In = "Log In";
		public const string Password = "Password";
		public const string Remembe_me = "Remembe me";
		public const string Sign_up = "Sign up";
		public const string 语言 = "语言";
	}
}