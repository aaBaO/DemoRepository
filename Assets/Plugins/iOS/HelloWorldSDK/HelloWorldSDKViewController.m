#include "HelloWorldSDKViewController.h"

@implementation HelloWorldSDKViewController

-(void) viewDidLoad{
    [super viewDidLoad];
    self.view.backgroundColor = [UIColor whiteColor];
    
    UIButton *btnBack = [UIButton buttonWithType:UIButtonTypeSystem];
    [btnBack setTitle:@"返回" forState:UIControlStateNormal];
    btnBack.center = CGPointMake(0.5f * self.view.bounds.size.width, 0.5f * self.view.bounds.size.height - 100);
    [btnBack addTarget:self action:@selector(backUnityScene:) forControlEvents:UIControlEventTouchUpInside];
    [self.view addSubview:btnBack];
    
    UILabel *title = [[UILabel alloc]init];
    title.text = @"Hello World";
    title.center = CGPointMake(0.5f * self.view.bounds.size.width, 100);
    [self.view addSubview:title];
    
    self.view.backgroundColor = [UIColor grayColor];
}

- (void)backUnityScene:(id)sender {
    NSLog(@"[HelloWorldSDK] Back to unity scene");
}
@end
