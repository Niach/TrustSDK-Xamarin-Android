using Android.App;
using Android.Widget;
using Android.OS;
using Trust;
using Android.Content;
using Android.Runtime;
using System;

namespace SampleApp
{
    [Activity(Label = "SampleApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private Call messageCall = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button signButton = FindViewById<Button>(Resource.Id.sign_message);
            signButton.Click += (object sender, System.EventArgs e) => {
                Trust.Trust.SignMessage()
                     .Message("TestMessage")
                     .IsPersonal(false)
                     .Call(this);
            };
            Button transButton = FindViewById<Button>(Resource.Id.sign_transaction);
            transButton.Click += (object sender, EventArgs e) => {
                Trust.Trust.SignTransaction()
                     .Recipient(new Trust.Core.Entity.Address("0x5cf56170cD73545231Faa304986AA496b4847776"))
                     .GasPrice(new Java.Math.BigInteger("16"))
                     .GasLimit(21000)
                     .Value(new Java.Math.BigDecimal("0.3").Multiply(Java.Math.BigDecimal.Ten.Pow(18)).ToBigInteger())
                     .Nonce(0)
                     .Payload("0x")
                     .Call(this);
            };
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            if(messageCall != null) {
                outState.PutParcelable("message_sign_call", messageCall);
            }
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Trust.Trust.OnActivityResult(requestCode, (int)resultCode, data).Subscribe(
                new OnSuccesListener(this), new OnFailureListener(this));

        }
    }
    public class OnSuccesListener : Java.Lang.Object, IOnSuccessListener
    {
        private Context context;
        public OnSuccesListener(Context context){
            this.context = context;
        }
        public void OnSuccess(IRequest p0, string p1)
        {
            Toast.MakeText(context, "Success: " +  p1, ToastLength.Long);
        }
    }
    public class OnFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private Context context;
        public OnFailureListener(Context context)
        {
            this.context = context;
        }

        public void OnFail(IRequest p0, int p1)
        {
            Toast.MakeText(context, "Failure: " + p1, ToastLength.Long);
        }
    }

}

