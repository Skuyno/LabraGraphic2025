using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class Task1{
    public static void Run(){
        int H = 100;
        int W = 100;

        CreateBlackImage(H,W);
        CreateWhiteImage(H,W);
        CreateRedImage(H,W);
        CreateGradientImage(H,W);
    }

    static void CreateBlackImage(int H, int W){
        using(var image = new Image<L8>(W,H)){
            for(int y = 0;y < H; y++){
                for(int x = 0; x < W; x++){
                    image[x,y] = new L8(0);
                }
            }
            image.Save("black_image.png");
        }
    }
    
    static void CreateWhiteImage(int H, int W){
        using(var image = new Image<L8>(W,H)){
            for(int y = 0;y < H; y++){
                for(int x = 0; x < W; x++){
                    image[x,y] = new L8(255);
                }
            }
            image.Save("white_image.png");
        }
    }
    
    static void CreateRedImage(int H, int W){
        using(var image = new Image<Rgba32>(W,H)){
            for(int y = 0;y < H; y++){
                for(int x = 0; x < W; x++){
                    image[x,y] = new Rgba32(255, 0, 0, 255);
                }
            }
            image.Save("red_image.png");
        }
    }
    
    static void CreateGradientImage(int H, int W){
        using (var image = new Image<Rgba32>(W, H)){
        for (int y = 0; y < H; y++){
            for (int x = 0; x < W; x++){
                // По какой-то интовые значение не работают
                byte value = (byte)((x + y) % 256);
                image[x, y] = new Rgba32(value, value, value, 255);
            }
        }
        image.Save("gradient_image.png");
    }
    }
}