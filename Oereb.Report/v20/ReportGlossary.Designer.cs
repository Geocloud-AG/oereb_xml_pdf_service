namespace Oereb.Report.v20
{
    partial class ReportGlossary
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportGlossary));
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.LogoFederal = new Telerik.Reporting.PictureBox();
            this.CantonalLogo = new Telerik.Reporting.PictureBox();
            this.MunicipalityLogo = new Telerik.Reporting.PictureBox();
            this.LogoPLRCadastre = new Telerik.Reporting.PictureBox();
            this.shape7 = new Telerik.Reporting.Shape();
            this.detail = new Telerik.Reporting.DetailSection();
            this.Title = new Telerik.Reporting.TextBox();
            this.LIGlossary = new Telerik.Reporting.List();
            this.PGlossary = new Telerik.Reporting.Panel();
            this.GlossaryItem = new Telerik.Reporting.HtmlTextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.GlossaryItemsDs = new Telerik.Reporting.ObjectDataSource();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.FootCreationDate = new Telerik.Reporting.TextBox();
            this.FootExtractIdentifier = new Telerik.Reporting.TextBox();
            this.shape9 = new Telerik.Reporting.Shape();
            this.FootPageInformation = new Telerik.Reporting.TextBox();
            this.ExtractDs = new Telerik.Reporting.ObjectDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.LogoFederal,
            this.CantonalLogo,
            this.MunicipalityLogo,
            this.LogoPLRCadastre,
            this.shape7});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // LogoFederal
            // 
            this.LogoFederal.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.LogoFederal.MimeType = "";
            this.LogoFederal.Name = "LogoFederal";
            this.LogoFederal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.4000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(1.3000998497009277D));
            this.LogoFederal.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.LogoFederal.Value = "= Fields.Extract.FederalLogo";
            // 
            // CantonalLogo
            // 
            this.CantonalLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.9795832633972168D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.CantonalLogo.MimeType = "";
            this.CantonalLogo.Name = "CantonalLogo";
            this.CantonalLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(1.2999999523162842D));
            this.CantonalLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.CantonalLogo.Value = "= Fields.Extract.CantonalLogo";
            // 
            // MunicipalityLogo
            // 
            this.MunicipalityLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.4985418319702148D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.MunicipalityLogo.MimeType = "";
            this.MunicipalityLogo.Name = "MunicipalityLogo";
            this.MunicipalityLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(1.2999999523162842D));
            this.MunicipalityLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.MunicipalityLogo.Value = "= Fields.Extract.MunicipalityLogo";
            // 
            // LogoPLRCadastre
            // 
            this.LogoPLRCadastre.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.890625D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.LogoPLRCadastre.MimeType = "";
            this.LogoPLRCadastre.Name = "LogoPLRCadastre";
            this.LogoPLRCadastre.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5D), Telerik.Reporting.Drawing.Unit.Cm(1D));
            this.LogoPLRCadastre.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.LogoPLRCadastre.Value = "= Fields.Extract.LogoPLRCadastre";
            // 
            // shape7
            // 
            this.shape7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.7991665601730347D));
            this.shape7.Name = "shape7";
            this.shape7.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.10000000149011612D));
            this.shape7.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.20000000298023224D);
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(24.100000381469727D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Title,
            this.LIGlossary});
            this.detail.Name = "detail";
            // 
            // Title
            // 
            this.Title.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.Title.Name = "Title";
            this.Title.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.100000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.Title.Style.Font.Bold = true;
            this.Title.Style.Font.Name = "Cadastra";
            this.Title.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(15D);
            resources.ApplyResources(this.Title, "Title");
            // 
            // LIGlossary
            // 
            this.LIGlossary.Bindings.Add(new Telerik.Reporting.Binding("DataSource", "= Fields.GlossaryItems"));
            this.LIGlossary.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D)));
            this.LIGlossary.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D)));
            this.LIGlossary.Body.SetCellContent(0, 0, this.PGlossary);
            tableGroup1.Name = "ColumnGroup";
            this.LIGlossary.ColumnGroups.Add(tableGroup1);
            this.LIGlossary.DataSource = this.GlossaryItemsDs;
            this.LIGlossary.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.PGlossary});
            this.LIGlossary.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.2000000476837158D));
            this.LIGlossary.Name = "LIGlossary";
            tableGroup2.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup2.Name = "DetailGroup";
            this.LIGlossary.RowGroups.Add(tableGroup2);
            this.LIGlossary.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            // 
            // PGlossary
            // 
            this.PGlossary.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.GlossaryItem,
            this.shape1});
            this.PGlossary.Name = "PGlossary";
            this.PGlossary.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            // 
            // GlossaryItem
            // 
            this.GlossaryItem.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.0999998152256012D));
            this.GlossaryItem.Name = "GlossaryItem";
            this.GlossaryItem.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.390625D), Telerik.Reporting.Drawing.Unit.Cm(0.39999952912330627D));
            this.GlossaryItem.Style.Font.Name = "Cadastra";
            this.GlossaryItem.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            resources.ApplyResources(this.GlossaryItem, "GlossaryItem");
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.49989920854568481D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.10000000149011612D));
            this.shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.20000000298023224D);
            // 
            // GlossaryItemsDs
            // 
            this.GlossaryItemsDs.DataSource = typeof(Oereb.Report.v20.ReportExtract.GlossaryItem);
            this.GlossaryItemsDs.Name = "GlossaryItemsDs";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.40000000596046448D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.FootCreationDate,
            this.FootExtractIdentifier,
            this.shape9,
            this.FootPageInformation});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // FootCreationDate
            // 
            this.FootCreationDate.Format = "{0:G}";
            this.FootCreationDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.10000000149011612D));
            this.FootCreationDate.Name = "FootCreationDate";
            this.FootCreationDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9000003337860107D), Telerik.Reporting.Drawing.Unit.Cm(0.25D));
            this.FootCreationDate.Style.Font.Name = "Cadastra";
            this.FootCreationDate.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            resources.ApplyResources(this.FootCreationDate, "FootCreationDate");
            // 
            // FootExtractIdentifier
            // 
            this.FootExtractIdentifier.Format = "";
            this.FootExtractIdentifier.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.2000000476837158D), Telerik.Reporting.Drawing.Unit.Cm(0.10000000149011612D));
            this.FootExtractIdentifier.Name = "FootExtractIdentifier";
            this.FootExtractIdentifier.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0.25D));
            this.FootExtractIdentifier.Style.Font.Name = "Cadastra";
            this.FootExtractIdentifier.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            resources.ApplyResources(this.FootExtractIdentifier, "FootExtractIdentifier");
            // 
            // shape9
            // 
            this.shape9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.shape9.Name = "shape9";
            this.shape9.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.05000000074505806D));
            this.shape9.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.800000011920929D);
            // 
            // FootPageInformation
            // 
            this.FootPageInformation.Format = "";
            this.FootPageInformation.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.5D), Telerik.Reporting.Drawing.Unit.Cm(0.10000000149011612D));
            this.FootPageInformation.Name = "FootPageInformation";
            this.FootPageInformation.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9000003337860107D), Telerik.Reporting.Drawing.Unit.Cm(0.25D));
            this.FootPageInformation.Style.Font.Name = "Cadastra";
            this.FootPageInformation.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(6D);
            this.FootPageInformation.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            resources.ApplyResources(this.FootPageInformation, "FootPageInformation");
            // 
            // ExtractDs
            // 
            this.ExtractDs.DataMember = "GetReportExtract";
            this.ExtractDs.DataSource = typeof(Oereb.Report.v20.ReportExtract);
            this.ExtractDs.Name = "ExtractDs";
            // 
            // ReportGlossary
            // 
            this.DataSource = this.ExtractDs;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1});
            this.Name = "ReportGlossar";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(18D), Telerik.Reporting.Drawing.Unit.Mm(18D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(12D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Style.Font.Name = "Cadastra";
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(17.399999618530273D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.ObjectDataSource ExtractDs;
        private Telerik.Reporting.PictureBox LogoFederal;
        private Telerik.Reporting.PictureBox CantonalLogo;
        private Telerik.Reporting.PictureBox MunicipalityLogo;
        private Telerik.Reporting.PictureBox LogoPLRCadastre;
        private Telerik.Reporting.Shape shape7;
        private Telerik.Reporting.TextBox Title;
        private Telerik.Reporting.List LIGlossary;
        private Telerik.Reporting.Panel PGlossary;
        private Telerik.Reporting.ObjectDataSource GlossaryItemsDs;
        private Telerik.Reporting.HtmlTextBox GlossaryItem;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.TextBox FootCreationDate;
        private Telerik.Reporting.TextBox FootExtractIdentifier;
        private Telerik.Reporting.Shape shape9;
        private Telerik.Reporting.TextBox FootPageInformation;
    }
}