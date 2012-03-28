﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheAirline.Model.AirlinerModel;
using TheAirline.Model.GeneralModel;
using TheAirline.GraphicsModel.PageModel.GeneralModel;

namespace TheAirline.GraphicsModel.UserControlModel.PopUpWindowsModel
{
    /// <summary>
    /// Interaction logic for PopUpOrderAirliners.xaml
    /// </summary>
    public partial class PopUpOrderAirliners : PopUpWindow
    {
        private AirlinerType Airliner;
        private ucNumericUpDown nudAirliners;
        public static object ShowPopUp(AirlinerType airliner)
        {
            PopUpWindow window = new PopUpOrderAirliners(airliner);
            window.ShowDialog();

            return window.Selected;
        }
        public PopUpOrderAirliners(AirlinerType airliner)
        {
            this.Airliner = airliner;

            InitializeComponent();

            this.Width = 300;

            this.Height = 110;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            this.Title = this.Airliner.Name;

            StackPanel mainPanel = new StackPanel();
            mainPanel.Margin = new Thickness(10, 10, 10, 10);

            ListBox lbOrder = new ListBox();
            lbOrder.ItemContainerStyleSelector = new ListBoxItemStyleSelector();
            lbOrder.SetResourceReference(ListBox.ItemTemplateProperty, "QuickInfoItem");

            mainPanel.Children.Add(lbOrder);

            nudAirliners = new ucNumericUpDown();
            nudAirliners.Height = 30;
            nudAirliners.MaxValue = 10;
            nudAirliners.Value = 1;
            //nudGates.ValueChanged += new RoutedPropertyChangedEventHandler<decimal>(nudGates_ValueChanged);
            nudAirliners.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            nudAirliners.MinValue = 1;

            lbOrder.Items.Add(new QuickInfoValue("Orders", nudAirliners));


            WrapPanel panelButtons = new WrapPanel();
            panelButtons.Margin = new Thickness(0, 10, 0, 0);

            mainPanel.Children.Add(panelButtons);



            Button btnOk = new Button();
            btnOk.Uid = "100";
            btnOk.SetResourceReference(Button.StyleProperty, "RoundedButton");
            btnOk.Height = Double.NaN;
            btnOk.Width = Double.NaN;
            btnOk.Content = Translator.GetInstance().GetString("General", btnOk.Uid);
            btnOk.Click += new RoutedEventHandler(btnOk_Click);
            btnOk.SetResourceReference(Button.BackgroundProperty, "ButtonBrush");

            panelButtons.Children.Add(btnOk);
                       
            Button btnCancel = new Button();
            btnCancel.Uid = "101";
            btnCancel.SetResourceReference(Button.StyleProperty, "RoundedButton");
            btnCancel.Height = Double.NaN;
            btnCancel.Width = Double.NaN;
            btnCancel.Content = Translator.GetInstance().GetString("General", btnCancel.Uid);
            btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
            btnCancel.Margin = new Thickness(5, 0, 0, 0);
            btnCancel.SetResourceReference(Button.BackgroundProperty, "ButtonBrush");

            panelButtons.Children.Add(btnCancel);
            this.Content = mainPanel;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Selected = null;
            this.Close();
        }
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            int airliners = Convert.ToInt32(nudAirliners.Value);
            this.Selected = airliners;
            this.Close();
           
        }
    }
}