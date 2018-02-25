#import "iOSBridgePlugin.h"
#import "HelloWorldSDKViewController.h"

void CalliOSNativeFunction(){
    NSLog(@"[iOS Native] I am running!");
    HelloWorldSDKViewController *helloworldsdkViewController = [[HelloWorldSDKViewController alloc]init];
    [GetAppController().rootViewController presentViewController:helloworldsdkViewController
                                                        animated:true
                                                      completion:nil];
}
