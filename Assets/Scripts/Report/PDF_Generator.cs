using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UnityEngine.Networking;

public class PDF_Generator : MonoBehaviour
{
    public ScreenShotHighRes screenshotGO;

    public iTextSharp.text.Image printImage;

    [HideInInspector]
    public byte[] imageByte;

    [HideInInspector]
    public byte[] pdfbytes;

    string fileName;

    public MainRegister_Manager mainRegisterManager;

    public IEnumerator ClickCoroutine()
    {
        //WAIT UNTIL TAKESCREENSHOT COROUTINE IS FINISHED
        if (screenshotGO)
        {
            yield return screenshotGO.TakeScreenShot();

            imageByte = screenshotGO.byteTest;

            Debug.Log("Ended TakeScreenShot Func");
        }

        try
        {
            BaseFont bold_text = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            //PORTRAIT
            Document doc = new Document(PageSize.A4, 10, 10, 30, 5);
            //LANDSCAPE
            // Document doc = new Document(PageSize.A4.Rotate(), 10, 10, 30, 5);

            DateTime dt = DateTime.Now;
            fileName = "Report_" + DataStorage.instance.userName + "_" + dt.Year + "_" + dt.Month + "_" + dt.Day + "_" + dt.Hour + dt.Minute + dt.Second + ".pdf";

            string pathToInstance = Path.Combine(Application.persistentDataPath, fileName);

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pathToInstance, FileMode.Create));

            doc.Open();

            doc.AddTitle("");
            doc.AddAuthor("Smartidea");
            doc.AddSubject("Smartidea PDF");
            doc.AddCreator("Smartidea PDF");

            iTextSharp.text.Font bold_18 = new iTextSharp.text.Font(bold_text, 15, 1);
            iTextSharp.text.Font simple_12 = new iTextSharp.text.Font(bold_text, 10);
            iTextSharp.text.Font bold_12 = new iTextSharp.text.Font(bold_text, 10, 1);

            WritePdf(doc, 0, 1, bold_18, simple_12, bold_12);

            doc.Close();

            StartCoroutine(SendToServer(pathToInstance, fileName));

            DataStorage.instance.DeleteAllData();

            if (mainRegisterManager)
            {
                if (mainRegisterManager.gameEnded)
                {
                    mainRegisterManager.HideConfirmScreen();
                    mainRegisterManager.PlayToRegister();
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    IEnumerator SendToServer(string file, string fileName)
    {
        pdfbytes = File.ReadAllBytes(file);

        string fileNew = Convert.ToBase64String(pdfbytes);

        WWWForm form = new WWWForm();

        form.AddField("fileBytes", fileNew);
        form.AddField("fileName", fileName);

        // Debug.Log("FOIFOI");

        yield return ServerRequest(form);

        Debug.Log("Sent DOC to Server!");
    }

    IEnumerator ServerRequest(WWWForm form)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/upload-pdf", form))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Received: " + request.downloadHandler.text);

                foreach (var fileToDelete in Directory.GetFiles(Application.persistentDataPath))
                {
                    FileInfo file_info = new FileInfo(fileToDelete);
                    try
                    {
                        Debug.Log(file_info);
                        file_info.Delete();
                    }
                    catch
                    {
                        Debug.Log("File Delete Error! Probably is in use.");
                    }
                }
            }
        }
    }

    public void GeneratePDF()
    {
        StartCoroutine(ClickCoroutine());
    }

    //空行
    public void AddBlank(Document doc)
    {
        Paragraph blank = new Paragraph(" ");
        doc.Add(blank);
    }

    //写PDF
    public void WritePdf(Document doc, int logonum, int statusnum, iTextSharp.text.Font bold_18, iTextSharp.text.Font simple_12, iTextSharp.text.Font bold_12)
    {
        if (screenshotGO)
        {
            iTextSharp.text.Image IMG = iTextSharp.text.Image.GetInstance(imageByte);

            iTextSharp.text.pdf.PdfPCell imgCell1 = new iTextSharp.text.pdf.PdfPCell();
            imgCell1.AddElement(new Chunk(IMG, 0, 0));
            imgCell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;

            IMG.ScalePercent(30f);

            doc.Add(IMG);
        }

        string reportParagraph = "";

        foreach (string report in DataStorage.instance.reportList)
        {
            reportParagraph += (report + "\n");
        }

        Paragraph reportTexts = new Paragraph(reportParagraph);
        reportTexts.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
        doc.Add(reportTexts);

        AddBlank(doc);

        //账单基本信息
        // PdfPTable T1 = new PdfPTable(1);

        // PdfPCell invoce1 = new PdfPCell(new Phrase("Invoice # " + no_text.text, bold_18));
        // invoce1.HorizontalAlignment = 0;
        // invoce1.Border = 0;
        // invoce1.PaddingBottom = 5f;
        // invoce1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T1.AddCell(invoce1);

        // PdfPCell invoce2 = new PdfPCell(new Phrase("Invoice Date : " + date.text, simple_12));
        // invoce2.HorizontalAlignment = 0;
        // invoce2.Border = 0;
        // invoce2.PaddingBottom = 5f;
        // invoce2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T1.AddCell(invoce2);

        // PdfPCell invoce3 = new PdfPCell(new Phrase("Due Date : " + due_date.text, simple_12));
        // invoce3.HorizontalAlignment = 0;
        // invoce3.Border = 0;
        // invoce3.PaddingBottom = 5f;
        // invoce3.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T1.AddCell(invoce3);

        // doc.Add(T1);
        // AddBlank(doc);

        //账单人信息
        // PdfPTable T2 = new PdfPTable(1);

        // PdfPCell details1 = new PdfPCell(new Phrase("Invoiced To : ", bold_18));
        // details1.HorizontalAlignment = 0;
        // details1.Border = 0;
        // details1.PaddingBottom = 3f;
        // T2.AddCell(details1);

        // PdfPCell details2 = new PdfPCell(new Phrase(to_company.text, simple_12));
        // details2.HorizontalAlignment = 0;
        // details2.Border = 0;
        // details2.PaddingBottom = 3f;
        // T2.AddCell(details2);

        // PdfPCell details3 = new PdfPCell(new Phrase("ATTN : " + to_people.text, simple_12));
        // details3.HorizontalAlignment = 0;
        // details3.Border = 0;
        // details3.PaddingBottom = 3f;
        // T2.AddCell(details3);

        // PdfPCell details4 = new PdfPCell(new Phrase(to_address1.text, simple_12));
        // details4.HorizontalAlignment = 0;
        // details4.Border = 0;
        // details4.PaddingBottom = 3f;
        // T2.AddCell(details4);

        // PdfPCell details5 = new PdfPCell(new Phrase(to_address2.text, simple_12));
        // details5.HorizontalAlignment = 0;
        // details5.Border = 0;
        // details5.PaddingBottom = 3f;
        // T2.AddCell(details5);

        // doc.Add(T2);
        // AddBlank(doc);

        //账单明细
        // PdfPTable T3 = new PdfPTable(4);

        // PdfPCell des_title1 = new PdfPCell(new Phrase("Description", bold_18));
        // des_title1.Colspan = 3;
        // des_title1.HorizontalAlignment = 1;
        // des_title1.PaddingBottom = 5f;
        // des_title1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // des_title1.Border = 0;
        // T3.AddCell(des_title1);

        // PdfPCell des_title2 = new PdfPCell(new Phrase("Total (AUD)", bold_18));
        // des_title2.Colspan = 1;
        // des_title2.HorizontalAlignment = 1;
        // des_title2.PaddingBottom = 5f;
        // des_title2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // des_title2.Border = 0;
        // T3.AddCell(des_title2);

        // if (des_1.text != null)
        // {
        //     PdfPCell des1_1 = new PdfPCell(new Phrase(des_1.text, simple_12));
        //     des1_1.Colspan = 3;
        //     des1_1.HorizontalAlignment = 0;
        //     des1_1.PaddingBottom = 5f;
        //     T3.AddCell(des1_1);

        //     PdfPCell des1_2 = new PdfPCell(new Phrase("$" + price_1.text, simple_12));
        //     des1_2.Colspan = 1;
        //     des1_2.HorizontalAlignment = 2;
        //     des1_2.PaddingBottom = 5f;
        //     T3.AddCell(des1_2);
        // }

        // if (des_2.text != null)
        // {
        //     PdfPCell des2_1 = new PdfPCell(new Phrase(des_2.text, simple_12));
        //     des2_1.Colspan = 3;
        //     des2_1.HorizontalAlignment = 0;
        //     des2_1.PaddingBottom = 5f;
        //     T3.AddCell(des2_1);

        //     PdfPCell des2_2 = new PdfPCell(new Phrase("$" + price_2.text, simple_12));
        //     des2_2.Colspan = 1;
        //     des2_2.HorizontalAlignment = 2;
        //     des2_2.PaddingBottom = 5f;
        //     T3.AddCell(des2_2);
        // }

        // if (des_3.text != null)
        // {
        //     PdfPCell des3_1 = new PdfPCell(new Phrase(des_3.text, simple_12));
        //     des3_1.Colspan = 3;
        //     des3_1.HorizontalAlignment = 0;
        //     des3_1.PaddingBottom = 5f;
        //     T3.AddCell(des3_1);

        //     PdfPCell des3_2 = new PdfPCell(new Phrase("$" + price_3.text, simple_12));
        //     des3_2.Colspan = 1;
        //     des3_2.HorizontalAlignment = 2;
        //     des3_2.PaddingBottom = 5f;
        //     T3.AddCell(des3_2);
        // }

        // if (des_4.text != null)
        // {
        //     PdfPCell des4_1 = new PdfPCell(new Phrase(des_4.text, simple_12));
        //     des4_1.Colspan = 3;
        //     des4_1.HorizontalAlignment = 0;
        //     des4_1.PaddingBottom = 5f;
        //     T3.AddCell(des4_1);

        //     PdfPCell des4_2 = new PdfPCell(new Phrase("$" + price_4.text, simple_12));
        //     des4_2.Colspan = 1;
        //     des4_2.HorizontalAlignment = 2;
        //     des4_2.PaddingBottom = 5f;
        //     T3.AddCell(des4_2);
        // }

        // if (des_5.text != null)
        // {
        //     PdfPCell des5_1 = new PdfPCell(new Phrase(des_5.text, simple_12));
        //     des5_1.Colspan = 3;
        //     des5_1.HorizontalAlignment = 0;
        //     des5_1.PaddingBottom = 5f;
        //     T3.AddCell(des5_1);

        //     PdfPCell des5_2 = new PdfPCell(new Phrase("$" + price_5.text, simple_12));
        //     des5_2.Colspan = 1;
        //     des5_2.HorizontalAlignment = 2;
        //     des5_2.PaddingBottom = 5f;
        //     T3.AddCell(des5_2);
        // }

        // PdfPCell total1_1 = new PdfPCell(new Phrase("Sub Total ", bold_12));
        // total1_1.Colspan = 3;
        // total1_1.HorizontalAlignment = 2;
        // total1_1.PaddingBottom = 5f;
        // total1_1.Border = 0;
        // total1_1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total1_1);

        // PdfPCell total1_2 = new PdfPCell(new Phrase("$" + subtotal.text, bold_12));
        // total1_2.Colspan = 1;
        // total1_2.HorizontalAlignment = 1;
        // total1_2.PaddingBottom = 5f;
        // total1_2.Border = 0;
        // total1_2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total1_2);

        // PdfPCell total2_1 = new PdfPCell(new Phrase("GST(10%) ", bold_12));
        // total2_1.Colspan = 3;
        // total2_1.HorizontalAlignment = 2;
        // total2_1.PaddingBottom = 5f;
        // total2_1.Border = 0;
        // total2_1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total2_1);

        // PdfPCell total2_2 = new PdfPCell(new Phrase("$" + gst.text, bold_12));
        // total2_2.Colspan = 1;
        // total2_2.HorizontalAlignment = 1;
        // total2_2.PaddingBottom = 5f;
        // total2_2.Border = 0;
        // total2_2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total2_2);

        // PdfPCell total3_1 = new PdfPCell(new Phrase("Credit ", bold_12));
        // total3_1.Colspan = 3;
        // total3_1.HorizontalAlignment = 2;
        // total3_1.PaddingBottom = 5f;
        // total3_1.Border = 0;
        // total3_1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total3_1);

        // PdfPCell total3_2 = new PdfPCell(new Phrase("$" + credit.text, bold_12));
        // total3_2.Colspan = 1;
        // total3_2.HorizontalAlignment = 1;
        // total3_2.PaddingBottom = 5f;
        // total3_2.Border = 0;
        // total3_2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total3_2);

        // PdfPCell total4_1 = new PdfPCell(new Phrase("Discount ", bold_12));
        // total4_1.Colspan = 3;
        // total4_1.HorizontalAlignment = 2;
        // total4_1.PaddingBottom = 5f;
        // total4_1.Border = 0;
        // total4_1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total4_1);

        // PdfPCell total4_2 = new PdfPCell(new Phrase("$" + discount.text, bold_12));
        // total4_2.Colspan = 1;
        // total4_2.HorizontalAlignment = 1;
        // total4_2.PaddingBottom = 5f;
        // total4_2.Border = 0;
        // total4_2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total4_2);

        // PdfPCell total5_1 = new PdfPCell(new Phrase("Total ", bold_12));
        // total5_1.Colspan = 3;
        // total5_1.HorizontalAlignment = 2;
        // total5_1.PaddingBottom = 5f;
        // total5_1.Border = 0;
        // total5_1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total5_1);

        // PdfPCell total5_2 = new PdfPCell(new Phrase("$" + total.text, bold_12));
        // total5_2.Colspan = 1;
        // total5_2.HorizontalAlignment = 1;
        // total5_2.PaddingBottom = 5f;
        // total5_2.Border = 0;
        // total5_2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T3.AddCell(total5_2);

        // doc.Add(T3);
        // AddBlank(doc);

        //转账信息
        // PdfPTable T4 = new PdfPTable(4);

        // PdfPCell trans_title = new PdfPCell(new Phrase("Transactions", bold_18));
        // trans_title.Colspan = 4;
        // trans_title.HorizontalAlignment = 0;
        // trans_title.Border = 0;
        // trans_title.PaddingBottom = 10f;
        // T4.AddCell(trans_title);

        // PdfPCell trans1_1 = new PdfPCell(new Phrase("Transaction Date", bold_12));
        // trans1_1.HorizontalAlignment = 1;
        // trans1_1.PaddingBottom = 5f;
        // trans1_1.Border = 0;
        // trans1_1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T4.AddCell(trans1_1);

        // PdfPCell trans1_2 = new PdfPCell(new Phrase("Gateway", bold_12));
        // trans1_2.HorizontalAlignment = 1;
        // trans1_2.PaddingBottom = 5f;
        // trans1_2.Border = 0;
        // trans1_2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T4.AddCell(trans1_2);

        // PdfPCell trans1_3 = new PdfPCell(new Phrase("Transaction ID", bold_12));
        // trans1_3.HorizontalAlignment = 1;
        // trans1_3.PaddingBottom = 5f;
        // trans1_3.Border = 0;
        // trans1_3.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T4.AddCell(trans1_3);

        // PdfPCell trans1_4 = new PdfPCell(new Phrase("Amount (AUD)", bold_12));
        // trans1_4.HorizontalAlignment = 1;
        // trans1_4.PaddingBottom = 5f;
        // trans1_4.Border = 0;
        // trans1_4.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T4.AddCell(trans1_4);

        // PdfPCell trans2_1 = new PdfPCell(new Phrase(transdate.text, simple_12));
        // trans2_1.HorizontalAlignment = 1;
        // trans2_1.PaddingBottom = 5f;
        // T4.AddCell(trans2_1);

        // PdfPCell trans2_2 = new PdfPCell(new Phrase(transway.text, simple_12));
        // trans2_2.HorizontalAlignment = 1;
        // trans2_2.PaddingBottom = 5f;
        // T4.AddCell(trans2_2);

        // PdfPCell trans2_3 = new PdfPCell(new Phrase(transid.text, simple_12));
        // trans2_3.HorizontalAlignment = 1;
        // trans2_3.PaddingBottom = 5f;
        // T4.AddCell(trans2_3);

        // PdfPCell trans2_4 = new PdfPCell(new Phrase("$" + transtotal.text, simple_12));
        // trans2_4.HorizontalAlignment = 1;
        // trans2_4.PaddingBottom = 5f;
        // T4.AddCell(trans2_4);

        // PdfPCell trans3_1 = new PdfPCell(new Phrase("Balance ", bold_12));
        // trans3_1.Colspan = 3;
        // trans3_1.HorizontalAlignment = 2;
        // trans3_1.PaddingBottom = 5f;
        // trans3_1.Border = 0;
        // trans3_1.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T4.AddCell(trans3_1);

        // PdfPCell trans3_2 = new PdfPCell(new Phrase("$" + transbalance.text, bold_12));
        // trans3_2.HorizontalAlignment = 1;
        // trans3_2.PaddingBottom = 5f;
        // trans3_2.Border = 0;
        // trans3_2.BackgroundColor = new iTextSharp.text.BaseColor(233, 233, 233);
        // T4.AddCell(trans3_2);

        // doc.Add(T4);
        // AddBlank(doc);

        //备注
        // PdfPTable T5 = new PdfPTable(1);

        // PdfPCell notes_title = new PdfPCell(new Phrase("Notes", bold_18));
        // notes_title.HorizontalAlignment = 0;
        // notes_title.Border = 0;
        // notes_title.PaddingBottom = 10f;
        // T5.AddCell(notes_title);

        // PdfPCell notes1 = new PdfPCell(new Phrase(other.text, simple_12));
        // notes1.HorizontalAlignment = 0;
        // notes1.PaddingBottom = 5f;
        // T5.AddCell(notes1);

        // doc.Add(T5);
    }

    // Use this for initialization
    void Start()
    {
        string pdfPath = Application.persistentDataPath;
        Debug.Log(pdfPath);

        Directory.CreateDirectory(pdfPath);
    }
}
