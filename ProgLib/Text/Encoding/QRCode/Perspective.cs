using System;
using System.Drawing;

namespace ProgLib.Text.Encoding.QRCode
{
    /// <summary>
    ///	Создать изображение QR кода для тестирования. Изображение преобразуется с помощью алгоритма перспективы.
    /// </summary>
    internal class Perspective
	{
	    private Double CenterX;
	    private Double CenterY;
	    private Double CosRot;
	    private Double SinRot;
	    private Double CamDist;
	    private Double CosX;
	    private Double SinX;
	    private Double CamVectY;
	    private Double CamVectZ;
	    private Double CamPosY;
	    private Double CamPosZ;

	    internal Perspective(Double CenterX, Double CenterY, Double ImageRot, Double CamDist, Double RotX)
	    {
	        // Center position
	        this.CenterX = CenterX;
	        this.CenterY = CenterY;

            // Image rotation
            Double RotRad = Math.PI * ImageRot / 180.0;
	        CosRot = Math.Cos(RotRad);
	        SinRot = Math.Sin(RotRad);
 
	        // Camera distance from QR Code
	        this.CamDist = CamDist;

            // X and Z axis rotation constants
            Double RotXRad = Math.PI * RotX / 180.0;
	        CosX = Math.Cos(RotXRad);
	        SinX = Math.Sin(RotXRad);

	        // Camera vector relative to qr code image
	        CamVectY = SinX;
	        CamVectZ = CosX;

	        // Camera position relative to qr code image
	        CamPosY =  CamDist * CamVectY;
	        CamPosZ =  CamDist * CamVectZ;

	        // Exit
	        return;
	    }
    
	    internal PointF ScreenPosition(Double QRPosX, Double QRPosY)
	    {
            // Rotation
            Double PosX = CosRot * QRPosX - SinRot * QRPosY;
            Double PosY = SinRot * QRPosX + CosRot * QRPosY;

            // Temp values for intersection calclulation
            Double CamQR = CamVectY * PosY;
            Double T = CamQR / (CamQR - CamDist);

            // Screen position relative to screen center
            Double ScrnPosX = CenterX + PosX * (1 - T);
            Double TempPosY = PosY + (CamPosY - PosY) * T;
            Double TempPosZ = CamPosZ * T; // - ScrnCenterZ;

		    // Rotate around x axis
		    Double ScrnPosY = CenterY + TempPosY * CosX - TempPosZ * SinX;
        
		    // Program test
		    #if DEBUG
		    Double ScrnPosZ = TempPosY * SinX + TempPosZ * CosX;
		    if (Math.Abs(ScrnPosZ) > 0.0001) throw new ApplicationException("Положение экрана Z должно быть нулевым");
		    #endif

		    return new PointF((float) ScrnPosX, (float) ScrnPosY);
        }

	    internal void GetPolygon(Double PosX, Double PosY, Double Side, PointF[] Polygon)
	    {
	        Polygon[0] = ScreenPosition(PosX, PosY);
	        Polygon[1] = ScreenPosition(PosX + Side, PosY);
	        Polygon[2] = ScreenPosition(PosX + Side, PosY + Side);
	        Polygon[3] = ScreenPosition(PosX, PosY + Side);

	        return;
	    }
	}
}
