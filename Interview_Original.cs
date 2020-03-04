/**
   The call log gets automatically populated with some infomation and the user enters other details. 
   This method saves all the varaibles that need to be added the the call log database. Keep in mind 
   that there is a DBLayer class that is not shown here. 
   There are 37 roles, such as CSR and LTC, and each role has a its own set of varaibles. This method displays 
   all 45 roles. Any of the chklstBoxNumNBCSR are not in the call log right now, but it will be added back soon.
   There is no more closing question. Do your best to refactor this method in the given time. 
 */

public void InsertUserCallNotesData()
        {

            CallLogUI calllog = new CallLogUI();
            string relatedRef = string.Empty;
            bool isGQorNoCaller = false;
            lstcallType1.Clear();
            lstcallType2.Clear();
            lstcallType3.Clear();
            lstcallType4.Clear();
            lstcallerName.Clear();
            try
            {
                if (isGeneralQuestion || isNoCallerSelected || isCallBackSelected)
                {
                    calllog.StartTime = DateTime.Now;
                    isGQorNoCaller = true;
                }
                //if (cbFRA.Items.Count > 0)
                Control[] cbFRAlList = grpCallerDetails.Controls.Find("cbFRA", true);
                if (cbFRAlList.Count() > 0)
                    calllog.FRCRCNumber = ((ComboBox)cbFRAlList[0]).Text;//cbFRA.Items[0].ToString();

                Control[] txtFRAList = grpCallerDetails.Controls.Find("txtFRA", true);
                if (txtFRAList.Count() > 0)
                    calllog.FRCRCNumber = ((TextBox)txtFRAList[0]).Text;
                Control[] txtPhoneList = grpCallerDetails.Controls.Find("txtPhone", true);
                if (txtPhoneList.Count() > 0)
                    calllog.CallBackPhone = ((TextBox)txtPhoneList[0]).Text;

                if (string.IsNullOrWhiteSpace(CommonConstants.GANumber.ToString())) //Added by Bala for NBCSR issue
                {
                    Control[] txtnoList = grpCallerDetails.Controls.Find("txtno", true);
                    if (txtnoList.Count() > 0)
                        calllog.GANumber = ((TextBox)txtnoList[0]).Text;
                }
                if (!string.IsNullOrWhiteSpace(CommonConstants.GANumber.ToString()))
                {
                    calllog.GANumber = CommonConstants.GANumber;
                }
                calllog.SearchType = txtbxCallType.Text;
                calllog.SearchValue = txtbxCallDetails.Text;
                if (CommonConstants.SelectedRole==POSRoles.LoansAndSurrender || CommonConstants.SelectedRole==POSRoles.VLSCOMM || CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.BT || CommonConstants.SelectedRole == POSRoles.PolicyMaintenance || CommonConstants.SelectedRole == POSRoles.NBCSR || 
                    CommonConstants.SelectedRole == POSRoles.SSA || CommonConstants.SelectedRole == POSRoles.CRR || CommonConstants.SelectedRole == POSRoles.SvcRep || CommonConstants.SelectedRole == POSRoles.DIRefund ||
                    CommonConstants.SelectedRole == POSRoles.ABSSenior || CommonConstants.SelectedRole == POSRoles.Ulife || 
                    CommonConstants.SelectedRole == POSRoles.IncomeBenefit || CommonConstants.SelectedRole == POSRoles.Deferred || CommonConstants.SelectedRole == POSRoles.ADM || 
                    CommonConstants.SelectedRole == POSRoles.LTC || CommonConstants.SelectedRole == POSRoles.FieldComp)
                {
                    if (!string.IsNullOrEmpty(CommonConstants.interactionIDKey))
                    {
                        calllog.InteractionID = CommonConstants.interactionIDKey;
                    }
                    else
                    {
                        calllog.InteractionID = string.Empty;
                    }
                }
                if (CommonConstants.SelectedRole == POSRoles.LoansAndSurrender || CommonConstants.SelectedRole == POSRoles.VLSCOMM || CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.BT || CommonConstants.SelectedRole == POSRoles.PolicyMaintenance || CommonConstants.SelectedRole == POSRoles.NBCSR ||
                    CommonConstants.SelectedRole == POSRoles.SSA || CommonConstants.SelectedRole == POSRoles.CRR || CommonConstants.SelectedRole == POSRoles.SvcRep || CommonConstants.SelectedRole == POSRoles.DIRefund ||
                    CommonConstants.SelectedRole == POSRoles.ABSSenior || CommonConstants.SelectedRole == POSRoles.Ulife ||
                    CommonConstants.SelectedRole == POSRoles.IncomeBenefit || CommonConstants.SelectedRole == POSRoles.Deferred || CommonConstants.SelectedRole == POSRoles.ADM ||
                    CommonConstants.SelectedRole == POSRoles.LTC || CommonConstants.SelectedRole == POSRoles.FieldComp)
                {
                    if (!string.IsNullOrEmpty(CommonConstants.callerID))
                    {
                        calllog.CallerID = CommonConstants.callerID;
                    }
                    else
                    {
                        calllog.CallerID = string.Empty;
                    }
                }
                if (CommonConstants.SelectedRole == POSRoles.LoansAndSurrender || CommonConstants.SelectedRole == POSRoles.VLSCOMM || CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.BT || CommonConstants.SelectedRole == POSRoles.PolicyMaintenance || CommonConstants.SelectedRole == POSRoles.NBCSR ||
                    CommonConstants.SelectedRole == POSRoles.IncomeBenefit || CommonConstants.SelectedRole == POSRoles.Deferred || CommonConstants.SelectedRole == POSRoles.ADM || CommonConstants.SelectedRole == POSRoles.LTC || CommonConstants.SelectedRole == POSRoles.FieldComp || CommonConstants.SelectedRole == POSRoles.CRR || CommonConstants.SelectedRole == POSRoles.SvcRep || CommonConstants.SelectedRole == POSRoles.DIRefund ||
                       CommonConstants.SelectedRole == POSRoles.ABSSenior || CommonConstants.SelectedRole == POSRoles.Ulife)
                {
                    if (!string.IsNullOrEmpty(CommonConstants.CallerIntent))
                    {
                        calllog.CallerIntent = CommonConstants.CallerIntent;
                    }
                    else
                    {
                        calllog.CallerIntent = string.Empty;
                    }
                }
                calllog.Role = CommonConstants.SelectedRole.ToString();
                for (int i = 0; i < grpCallerDetails.Controls.Count; i++)
                {
                    try
                    {
                        if (CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.LTC
                            || CommonConstants.SelectedRole == POSRoles.SHRC || CommonConstants.SelectedRole == POSRoles.NBCSR) //Added on 11/28/ -NBSCR
                        {
                            CheckBox chkBox = (CheckBox)grpCallerDetails.Controls[i];
                            if (chkBox.Checked == true)
                            {
                                //lstClrType.Add(chkBox.Text);
                                string ctrlname = "txt" + chkBox.Name.Substring(2);
                                Control[] txtList = grpCallerDetails.Controls.Find(ctrlname, true);
                                if (txtList.Count() > 0)
                                {
                                    lstcallerName.Add(((TextBox)txtList[0]).Text);
                                    //calllog.CallerName = ((TextBox)txtList[0]).Text;
                                }
                                else if (chkBox.Text.ToLower().Contains("assignee"))
                                {
                                    Control[] cbList = grpCallerDetails.Controls.Find("cbAssignee", true);
                                    if (cbList.Count() > 0)
                                    {
                                        lstcallerName.Add(((ComboBox)cbList[0]).Text);
                                        //calllog.CallerName = ((ComboBox)cbList[0]).Text;
                                    }

                                }
                                else if (chkBox.Text.ToLower().Contains("fra"))
                                {
                                    Control[] cbList = grpCallerDetails.Controls.Find("cbFRA", true);
                                    if (cbList.Count() > 0)
                                    {
                                        lstcallerName.Add(((ComboBox)cbList[0]).Text);
                                        //calllog.CallerName = ((ComboBox)cbList[0]).Text;
                                    }

                                }
                                else if (chkBox.Text.ToLower().Contains("fr"))
                                {
                                    Control[] cbList = grpCallerDetails.Controls.Find("cbFR", true);
                                    if (cbList.Count() > 0)
                                    {
                                        lstcallerName.Add(((ComboBox)cbList[0]).Text);
                                        //calllog.CallerName = ((ComboBox)cbList[0]).Text;
                                    }

                                }
                            }
                        }
                        else
                        {
                            RadioButton rb = (RadioButton)grpCallerDetails.Controls[i];
                            if (rb.Checked == true)
                            {
                                calllog.CallerType = rb.Text;
                                string ctrlname = "txt" + rb.Name.Substring(2);
                                Control[] txtList = grpCallerDetails.Controls.Find(ctrlname, true);
                                if (txtList.Count() > 0)
                                {
                                    calllog.CallerName = ((TextBox)txtList[0]).Text;
                                }
                                else if (rb.Text.ToLower().Contains("assignee"))
                                {
                                    Control[] cbList = grpCallerDetails.Controls.Find("cbAssignee", true);
                                    if (cbList.Count() > 0)
                                        calllog.CallerName = ((ComboBox)cbList[0]).Text;
                                }
                                else if (rb.Text.ToLower().Contains("fra"))
                                {
                                    Control[] cbList = grpCallerDetails.Controls.Find("cbFRA", true);
                                    if (cbList.Count() > 0)
                                        calllog.CallerName = ((ComboBox)cbList[0]).Text;
                                }
                                else if (rb.Text.ToLower().Contains("fr"))
                                {
                                    Control[] cbList = grpCallerDetails.Controls.Find("cbFR", true);
                                    if (cbList.Count() > 0)
                                        calllog.CallerName = ((ComboBox)cbList[0]).Text;
                                }
                                break;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Logging.LogErrorMsg(this.Name, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    }
                }

                for (int i = 0; i < gbCalltype.Controls.Count; i++)
                {
                    try
                    {
                        CheckBox chkbox = (CheckBox)gbCalltype.Controls[i];
                        if (chkbox.Checked == true)
                        {
                            //string strText = chkbox.Text;
                            if (chkbox.Name.Contains("Specify"))
                            {
                                //if (chkbox.Text.Contains("Transferred"))
                                //{
                                //    Control[] ctlList = gbCalltype.Controls.Find("txtTransferred", true);
                                //    calllog.TransferredToNum = ((TextBox)ctlList[0]).Text;
                                //}
                                if (chkbox.Text.Contains("Transfer"))
                                {
                                    Control[] ctlList = gbCalltype.Controls.Find("txtTransfer", true);
                                    if (ctlList.Length >= 1)
                                    {
                                        calllog.InternalTransfer = ((TextBox)ctlList[0]).Text;
                                    }
                                    if (isSkillAvailable)
                                    {
                                        Control[] ctlList1 = gbCalltype.Controls.Find("cmbSkill", true);
                                        calllog.SkillSet = ((ComboBox)ctlList1[0]).Text;
                                    }
                                    if (isReasonCodeAvailable)
                                    {
                                        Control[] ctlList1 = gbCalltype.Controls.Find("cmbReason", true);
                                        calllog.ReasonCode = ((ComboBox)ctlList1[0]).Text;
                                    }
                                    if (isInternalTransferAvailable)
                                    {
                                        Control[] ctlList1 = gbCalltype.Controls.Find("cmbInternalTransfer", true);
                                        calllog.InternalTransfer = ((ComboBox)ctlList1[0]).Text;
                                    }
                                }
                                else if (chkbox.Text.Contains("Other"))
                                {
                                    Control[] ctlList = gbCalltype.Controls.Find("txtOther", true);
                                    calllog.Others = ((TextBox)ctlList[0]).Text;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logging.LogErrorMsg(this.Name, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    }
                }


                string chklstBox1Item = string.Empty;
                if (CommonConstants.SelectedRole == POSRoles.Distribution || CommonConstants.SelectedRole == POSRoles.ADM 
                    || CommonConstants.SelectedRole == POSRoles.CSR
                    || CommonConstants.SelectedRole == POSRoles.LTC || CommonConstants.SelectedRole == POSRoles.NBCSR)         //nancy
                {
                    if (chklstBox1NBCSR.CheckedItems.Count > 0)
                    {
                        string chkboxList1_Item = chklstBox1NBCSR.SelectedItem.ToString();


                        if (!string.IsNullOrEmpty(chklstBox1Item))
                        {
                            chklstBox1Item = chklstBox1Item + "," + chkboxList1_Item;
                        }
                        else
                        {
                            chklstBox1Item = chkboxList1_Item;
                        }



                        //if (!string.IsNullOrEmpty(chkboxList1_Item))
                        //{
                        //    lstcallType.Add(chkboxList1_Item);
                        //}
                    }
                    if (chklstBox2NBCSR.Visible == true)
                    {
                        if (chklstBox2NBCSR.CheckedItems.Count > 0)
                        {
                            string chkboxList2_Item = chklstBox2NBCSR.SelectedItem.ToString();
                            if (!string.IsNullOrEmpty(chkboxList2_Item))
                            {
                                if (CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.LTC
                                    || CommonConstants.SelectedRole == POSRoles.NBCSR)
                                {
                                    lstcallType2.Add(chkboxList2_Item);
                                }
                                else
                                {
                                    lstcallType.Add(chkboxList2_Item);
                                }
                            }
                        }
                    }
                    if (chklstBox3NBCSR.Visible == true)
                    {
                        if (chklstBox3NBCSR.CheckedItems.Count > 0)
                        {
                            string chkboxList3_Item = chklstBox3NBCSR.SelectedItem.ToString();
                            if (CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.NBCSR)
                            {
                                lstcallType3.Add(chkboxList3_Item);
                            }
                            else
                            {
                                lstcallType.Add(chkboxList3_Item);
                            }
                        }
                    }
                    if (chklstBox4NBCSR.Visible == true)
                    {
                        if (chklstBox4NBCSR.CheckedItems.Count > 0)
                        {
                            string chkboxList4_Item = chklstBox4NBCSR.SelectedItem.ToString();
                            if (!string.IsNullOrEmpty(chkboxList4_Item))
                            {
                                if (CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.NBCSR)
                                {
                                    lstcallType4.Add(chkboxList4_Item);
                                }
                                else
                                {
                                    lstcallType.Add(chkboxList4_Item);
                                }
                            }
                        }
                    }
                }
                
                foreach (string str in lstQualifier)
                {
                        if (!string.IsNullOrWhiteSpace(calllog.Qualifier))
                            calllog.Qualifier = calllog.Qualifier + "," + str;
                        else
                            calllog.Qualifier = str;
                }


                foreach (string str in lstclosingQues)
                {
                    if (!string.IsNullOrWhiteSpace(calllog.ClosingQues))
                        calllog.ClosingQues = calllog.ClosingQues + "," + str;
                    else
                        calllog.ClosingQues = str;
                }

                string result = string.Empty;
                int index = 0;
                string result1 = string.Empty;
                string num = string.Empty;
                bool isNumberChanged = false;


                foreach (Control cn in gbCalltype.Controls)
                {
                    if (cn is ComboBox)
                    {
                        isNumberChanged = false;
                        string cmbBoxName = cn.Name;
                        string comboBoxNameWithNumber = string.Empty;
                        string commonCmbBoxName = string.Empty;
                        string cmbBoxNumber = string.Empty;
                        string number = string.Empty;
                        string callType = string.Empty;
                        string h = cmbBoxName.ElementAt(cmbBoxName.Length - 3).ToString();
                        if (h.Any(char.IsDigit))
                        {
                            commonCmbBoxName = cmbBoxName.Substring(0, cmbBoxName.Length - 3);
                            comboBoxNameWithNumber = cmbBoxName.Substring(0, cmbBoxName.Length - 2);
                            int index1 = cmbBoxName.IndexOf(commonCmbBoxName);
                            cmbBoxNumber = comboBoxNameWithNumber.Substring(index1 + (commonCmbBoxName.Length));
                            number = cmbBoxName.Substring(cmbBoxName.Length - 2);
                            callType = cmbBoxName.Substring(3, cmbBoxName.Length - 6);
                            //if (!number.Equals(num))
                            //{
                            //    num = number;
                            //    if (!string.IsNullOrEmpty(result1))
                            //    {
                            //        isNumberChanged = true;
                            //    }

                            //}
                        }
                        else
                        {
                            commonCmbBoxName = cmbBoxName.Substring(0, cmbBoxName.Length - 2);
                            comboBoxNameWithNumber = cmbBoxName.Substring(0, cmbBoxName.Length - 1);
                            int index1 = cmbBoxName.IndexOf(commonCmbBoxName);
                            cmbBoxNumber = comboBoxNameWithNumber.Substring(index1 + (commonCmbBoxName.Length));
                            number = cmbBoxName.Substring(cmbBoxName.Length - 1);
                            callType = cmbBoxName.Substring(3, cmbBoxName.Length - 5);
                        }



                        if (!(cmbBoxName.ToLower().Contains("skill") || cmbBoxName.ToLower().Contains("transfer") || cmbBoxName.ToLower().Contains("reason") || cmbBoxName.ToLower().Contains("documentrequest")))
                        {
                            Control[] ctlList = gbCalltype.Controls.Find("cb" + number, true);
                            CheckBox chkBox = ((CheckBox)ctlList[0]);
                            ComboBox cmb = (ComboBox)cn;
                            if (chkBox != null && chkBox.Checked == true)
                            {
                                //if (!string.IsNullOrEmpty(result1) && isNumberChanged)
                                //{
                                //    result1 = result1 + ",";
                                //}
                                //if (string.IsNullOrEmpty(result1) && cmb.SelectedItem != null)
                                //{
                                //    result1 = cmb.SelectedItem.ToString();
                                //}
                                //else if (!string.IsNullOrEmpty(result1) && cmb.SelectedItem != null)
                                //{
                                //    if (!result1.EndsWith(","))
                                //    {
                                //        result1 = result1 + ":" + cmb.SelectedItem.ToString();
                                //    }
                                //    else
                                //    {
                                //        result1 = result1 + cmb.SelectedItem.ToString();
                                //    }
                                //}

                                if (comboBoxNameWithNumber.Contains("1") && cmbBoxNumber.Equals("1"))
                                {
                                    if (!string.IsNullOrEmpty(callType) && cmb.SelectedItem != null)
                                    {
                                        lstcallType2.Add(cmb.SelectedItem.ToString());
                                    }
                                    else if (string.IsNullOrEmpty(callType) && cmb.SelectedItem != null)
                                    {
                                        //lstcallType1.Add(cmb.SelectedItem.ToString());
                                        if (!string.IsNullOrEmpty(result1))
                                        {
                                            result1 = result1 + "," + cmb.SelectedItem.ToString();
                                        }
                                        else
                                        {
                                            result1 = cmb.SelectedItem.ToString();
                                        }
                                    }
                                }
                                else if (comboBoxNameWithNumber.Contains("2") && cmbBoxNumber.Equals("2"))
                                {
                                    if (cmb.SelectedItem != null)
                                    {
                                        lstcallType2.Add(cmb.SelectedItem.ToString());
                                    }
                                }
                                else if (comboBoxNameWithNumber.Contains("3") && cmbBoxNumber.Equals("3"))
                                {
                                    if (cmb.SelectedItem != null)
                                    {
                                        lstcallType3.Add(cmb.SelectedItem.ToString());
                                    }
                                }
                                else if (comboBoxNameWithNumber.Contains("4") && cmbBoxNumber.Equals("4"))
                                {
                                    if (cmb.SelectedItem != null)
                                    {
                                        lstcallType4.Add(cmb.SelectedItem.ToString());
                                    }
                                }
                            }

                        }
                    }
                }

                //Added for CSR call type removal
                if (CommonConstants.SelectedRole == POSRoles.CSR && lstcallType.Count == 0)
                {
                    calllog.CallType = "";
                }

                //Added for NBCSR call type removal
                if (CommonConstants.SelectedRole == POSRoles.NBCSR && lstcallType.Count == 0)
                {
                    calllog.CallType = "";
                }

                foreach (string str in lstcallType)
                {
                    if (!string.IsNullOrWhiteSpace(calllog.CallType))
                        calllog.CallType = calllog.CallType + "," + str;
                    else
                        calllog.CallType = str;
                }

                foreach (string str in lstClrType)
                {
                    if (!string.IsNullOrWhiteSpace(calllog.CallerType))
                        calllog.CallerType = calllog.CallerType + "," + str;
                    else
                        calllog.CallerType = str;
                }


                foreach (string str in lstcallerName)
                {
                    if (!string.IsNullOrWhiteSpace(calllog.CallerName))
                        calllog.CallerName = calllog.CallerName + "," + str;
                    else
                        calllog.CallerName = str;
                }

                if (!string.IsNullOrEmpty(result1))
                {
                    if (!string.IsNullOrWhiteSpace(calllog.CallType))
                    {
                        calllog.CallType = calllog.CallType + "," + result1;
                    }
                    else
                    {
                        calllog.CallType = result1;
                    }
                }

                if (!string.IsNullOrEmpty(chklstBox1Item))
                {
                    if (!string.IsNullOrWhiteSpace(calllog.CallType))
                    {
                        calllog.CallType = calllog.CallType + "," + chklstBox1Item;
                    }
                    else
                    {
                        calllog.CallType = chklstBox1Item;
                    }
                }

                foreach (string str in lstcallType2)
                {
                    if (!string.IsNullOrWhiteSpace(calllog.CallType2))
                        calllog.CallType2 = calllog.CallType2 + "," + str;
                    else
                        calllog.CallType2 = str;
                }



                foreach (string str in lstcallType3)
                {
                    if (!string.IsNullOrWhiteSpace(calllog.CallType3))
                        calllog.CallType3 = calllog.CallType3 + "," + str;
                    else
                        calllog.CallType3 = str;
                }

                foreach (string str in lstcallType4)
                {
                    if (!string.IsNullOrWhiteSpace(calllog.CallType4))
                        calllog.CallType4 = calllog.CallType4 + "," + str;
                    else
                        calllog.CallType4 = str;
                }
                if (CommonConstants.SelectedRole == POSRoles.CSR)
                {
                    //List<string> list = new List<string>();
                    //list = chckdLstBoxPolicies.CheckedItems.Cast<string>().ToList();
                    //foreach (string item in list)
                    //{
                    //    if (string.IsNullOrEmpty(relatedRef))
                    //    {
                    //        relatedRef = item;
                    //    }
                    //    else
                    //    {
                    //        relatedRef = relatedRef+", " + item;
                    //    }
                    //}
                    //calllog.RelatedRef = relatedRef;
                    calllog.RelatedRef = varRelatedRef;
                }

                if ((CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.LTC
                    || CommonConstants.SelectedRole == POSRoles.SHRC) && isNoCallerSelected)
                {
                    if (calllog.CallType == null)
                        calllog.CallType = string.Empty;
                }

                if ((CommonConstants.SelectedRole == POSRoles.PolicyMaintenance || CommonConstants.SelectedRole == POSRoles.Distribution
                    || CommonConstants.SelectedRole == POSRoles.ADM || CommonConstants.SelectedRole == POSRoles.IncomeBenefit || CommonConstants.SelectedRole == POSRoles.CRR) && isNoCallerSelected)   //Added by Bala on 11/4/2019
                {
                    if (calllog.CallType == null)
                        calllog.CallType = string.Empty;
                }

                //Added for LS role
                if ((CommonConstants.SelectedRole==POSRoles.LoansAndSurrender) && (isNoCallerSelected || isCallBackSelected))
                {
                    if (calllog.CallType == null)
                        calllog.CallType = string.Empty;
                }
                string stradd = string.Empty;
                for (int i = 0; i <= lbxAdditionalSearch.Items.Count - 1; i++)
                {
                    if (stradd != string.Empty)
                    {
                        stradd = stradd + "," + lbxAdditionalSearch.Items[i].ToString();
                    }
                    else
                    {
                        stradd = lbxAdditionalSearch.Items[i].ToString();
                    }
                }
                calllog.AdditionalSearchNos = stradd;

                calllog.CallNotes = txtbxNotes.Text;
                calllog.TransferFrom = string.Empty;
                if (CommonConstants.Callid < 0)
                    lblsuccesMessage.Text = saveMessage;
                else
                    lblsuccesMessage.Text = updateMessage;
                lblsuccesMessage.ForeColor = Color.Green;

                dblayer.InsertUserCallNotesData(calllog, isGQorNoCaller);
                if (CommonConstants.Callid < 0)
                {
                    lblsuccesMessage.Text = failedMessage;
                    lblsuccesMessage.ForeColor = Color.Red;
                    EnableSaveButton();
                }
                lblsuccesMessage.Visible = true;

                //Thread t = new Thread(delegate()
                //{
                //    dblayer.InsertUserCallNotesData(calllog, isGQorNoCaller);
                //    SetFailedMessage();
                //});
                //t.Start();

            }
            catch (Exception ex)
            {
                Logging.LogErrorMsg(this.Name, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }