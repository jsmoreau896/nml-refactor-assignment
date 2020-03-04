/**
   Assignment Description:
   The call log gets automatically populated with some infomation and the user enters other details. 
   This method saves all the varaibles that need to be added the the call log database. Keep in mind 
   that there is a DBLayer class that is not shown here. 
   There are 37 roles, such as CSR and LTC, and each role has a its own set of varaibles. This method displays 
   all 45 roles. Any of the chklstBoxNumNBCSR are not in the call log right now, but it will be added back soon.
   There is no more closing question. Do your best to refactor this method in the given time. 
 */

class MyClass
{

    // Constants: Required Roles for Functionality
    public static readonly string[] lstInteractionRequiredRoles = {POSRoles.LoansAndSurrender,
                                             POSRoles.VLSCOMM,
                                             POSRoles.CSR,
                                             POSRoles.BT,
                                             POSRoles.PolicyMaintenance ,
                                             POSRoles.NBCSR,
                                             POSRoles.SSA,
                                             POSRoles.CRR,
                                             POSRoles.SvcRep,
                                             POSRoles.DIRefund,
                                             POSRoles.ABSSenior,
                                             POSRoles.Ulife,
                                             POSRoles.IncomeBenefit,
                                             POSRoles.Deferred,
                                             POSRoles.ADM,
                                             POSRoles.LTC,
                                             POSRoles.FieldComp
                                            };

    public static readonly string[] lstCallerRequiredRoles = {POSRoles.LoansAndSurrender,
                                             POSRoles.VLSCOMM,
                                             POSRoles.CSR,
                                             POSRoles.BT,
                                             POSRoles.PolicyMaintenance ,
                                             POSRoles.NBCSR,
                                             POSRoles.SSA,
                                             POSRoles.CRR,
                                             POSRoles.SvcRep,
                                             POSRoles.DIRefund,
                                             POSRoles.ABSSenior,
                                             POSRoles.Ulife,
                                             POSRoles.IncomeBenefit,
                                             POSRoles.Deferred,
                                             POSRoles.ADM,
                                             POSRoles.LTC,
                                             POSRoles.FieldComp
                                            };

    public static readonly string[] lstCallerIntentRequiredRoles = {POSRoles.LoansAndSurrender,
                                             POSRoles.VLSCOMM,
                                             POSRoles.CSR,
                                             POSRoles.BT,
                                             POSRoles.PolicyMaintenance ,
                                             POSRoles.NBCSR,
                                             POSRoles.SvcRep,
                                             POSRoles.DIRefund,
                                             POSRoles.ABSSenior,
                                             POSRoles.Ulife,
                                             POSRoles.IncomeBenefit,
                                             POSRoles.Deferred,
                                             POSRoles.ADM,
                                             POSRoles.LTC,
                                             POSRoles.FieldComp
                                            };

    public static readonly string[] lstCallerNameRequiredRoles = {POSRoles.CSR,
                                             POSRoles.LTC,
                                             POSRoles.SHRC,
                                             POSRoles.NBCSR
                                            };


    public static readonly string[] lstBox1ItemRequiredRoles = {POSRoles.Distribution,
                                             POSRoles.ADM,
                                             POSRoles.CSR,
                                             POSRoles.LTC,
                                             POSRoles.NBCSR
                                            };


    /********************************************************************
        Main Method
        Most likely wouldn't be in this class but it gives you a starting
        reference of what gets called first 
    *********************************************************************/
    static public void Main(String[] args)
    {
        Console.WriteLine("Starting the process...");
        InsertUserCallNotesData();
        Console.WriteLine("Process finished.");
    }


    /********************************************************************
        Insert User CallNotes Data Service
    *********************************************************************/
    public void InsertUserCallNotesData()
    {
        // Initalize and reset
        bool isGQorNoCaller = false;
        resetCallTypes();

        try
        {

            // If this call notes are for a general question, no caller selected
            // or call back is selected, then flip isGQorNoCaller to true
            if (IsGeneralQuestOrNoCallerOrCallBack())
                isGQorNoCaller = true;

            // Create the CallLogUI object
            CallLogUI calllog = createCallLogUI();

            // Set Success or Error message for UI
            if (CommonConstants.Callid < 0)
            {
                // Error Message
                lblsuccesMessage.Text = failedMessage;
                lblsuccesMessage.ForeColor = Color.Red;
                EnableSaveButton();
            }
            else
            {
                // Success Message
                // lblsuccesMessage.Text = updateMessage;  //TODO Please ask about this
                lblsuccesMessage.Text = saveMessage;
                lblsuccesMessage.ForeColor = Color.Green;
                lblsuccesMessage.Visible = true;
            }

            // Insert User Call Notes into Database
            dblayer.InsertUserCallNotesData(calllog, isGQorNoCaller);

        }
        catch (Exception ex)
        {
            // ERROR: Log Error
            Logging.LogErrorMsg(this.Name, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        }
    }



    /********************************************************************
        Create CallLogUI Object
    *********************************************************************/
    public CallLogUI createCallLogUI()
    {
        try
        {

            CallLogUI calllog = new CallLogUI();

            // Set start time for a General Question, No Caller Selected, or Call Back was selected
            if (IsGeneralQuestOrNoCallerOrCallBack())
                calllog.StartTime = DateTime.Now;


            // Populate the call log values
            calllog.FRCRCNumber = getCallerDetailsControlListValue("cbFRA", true, ComboBox.clazz);
            calllog.FRCRCNumber = getCallerDetailsControlListValue("txtFRA", true, TextBox.clazz);  // TODO look into two FRCRCNumber.  Verify rules
            calllog.CallBackPhone = getCallerDetailsControlListValue("txtFRA", true, TextBox.clazz);
            calllog.SearchType = txtbxCallType.Text;
            calllog.SearchValue = txtbxCallDetails.Text;
            calllog.Role = CommonConstants.SelectedRole.ToString();
            calllog.Qualifier = buildValueForList(calllog.Qualifier, lstQualifier);
            calllog.ClosingQues = buildValueForList(calllog.ClosingQues, lstclosingQues);


            if (string.IsNullOrWhiteSpace(CommonConstants.GANumber.ToString()))
                calllog.GANumber = getCallerControlListValue("txtno", true, TextBox.clazz);
            else
                calllog.GANumber = CommonConstants.GANumber;


            // If selected role matches the required list of roles then get and set the call log values
            if (isRequiredForSelectedRole(CommonConstants.SelectedRole, lstInteractionRequiredRoles))
                calllog.InteractionID = getValueWithNullCheck(CommonConstants.interactionIDKey);

            if (isRequiredForSelectedRole(CommonConstants.SelectedRole, lstCallerRequiredRoles))
                calllog.CallerID = getValueWithNullCheck(CommonConstants.callerID);

            if (isRequiredForSelectedRole(CommonConstants.SelectedRole, lstCallerIntentRequiredRoles))
                calllog.CallerIntent = getValueWithNullCheck(CommonConstants.CallerIntent);


            // For each Caller Detail record create the list of CallerNames or single value CallerName
            for (int i = 0; i < grpCallerDetails.Controls.Count; i++)
            {
                if (isRequiredForSelectedRole(CommonConstants.SelectedRole, lstCallerNameRequiredRoles))
                {
                    // Add items to lstcallerName array
                    CheckBox chkBox = (CheckBox)grpCallerDetails.Controls[i];
                    if (chkBox.Checked == true)
                    {
                        string ctrlname = "txt" + chkBox.Name.Substring(2);
                        if (hasRecords(findCallerDetails(ctrlname, true)))
                        {
                            lstcallerName.Add(getCallerDetailsControlListValue(ctrlname, true, TextBox.clazz));
                        }
                        else if (containsIgnoreCase(chkBox.Text, "assignee"))
                        {
                            lstcallerName.Add(getCallerDetailsControlListValue("cbAssignee", true, ComboBox.clazz));
                        }
                        else if (containsIgnoreCase(chkBox.Text, "fra"))
                        {
                            lstcallerName.Add(getCallerDetailsControlListValue("cbFRA", true, ComboBox.clazz));
                        }
                        else if (containsIgnoreCase(chkBox.Text, "fr"))
                        {
                            lstcallerName.Add(getCallerDetailsControlListValue("cbFR", true, ComboBox.clazz));
                        }
                    }
                }
                else
                {
                    // Set CallerName field
                    RadioButton rb = (RadioButton)grpCallerDetails.Controls[i];
                    if (rb.Checked == true)
                    {
                        calllog.CallerType = rb.Text; //TODO Review as this is being set above
                        string ctrlname = "txt" + rb.Name.Substring(2);
                        if (hasRecords(findCallerDetails(ctrlname, true)))
                        {
                            calllog.CallerName = getCallerDetailsControlListValue(ctrlname, true, TextBox.clazz);
                        }
                        else if (containsIgnoreCase(rb.Text, "assignee"))
                        {
                            calllog.CallerName = getCallerDetailsControlListValue("cbAssignee", true, ComboBox.clazz);
                        }
                        else if (containsIgnoreCase(chkBox.Text, "fra"))
                        {
                            calllog.CallerName = getCallerDetailsControlListValue("cbFRA", true, ComboBox.clazz);
                        }
                        else if (containsIgnoreCase(chkBox.Text, "fr"))
                        {
                            calllog.CallerName = getCallerDetailsControlListValue("cbFR", true, ComboBox.clazz);
                        }
                        break;
                    }
                }
            }


            // Loop through the Call Type array and set values
            for (int i = 0; i < gbCalltype.Controls.Count; i++)
            {
                CheckBox chkbox = (CheckBox)gbCalltype.Controls[i];
                if (chkbox.Checked == true && chkbox.Name.Contains("Specify"))
                {
                    if (chkbox.Text.Contains("Transfer"))
                    {
                        if (isSkillAvailable)
                            calllog.SkillSet = getCallerTypeControlListValue("cmbSkill", true, ComboBox);

                        if (isReasonCodeAvailable)
                            calllog.ReasonCode = getCallerTypeControlListValue("cmbReason", true, ComboBox);

                        if (isInternalTransferAvailable)
                            calllog.InternalTransfer = getCallerTypeControlListValue("cmbInternalTransfer", true, ComboBox);
                        else
                            calllog.InternalTransfer = getCallerTypeControlListValue("txtTransfer", true, TextBox);

                    }
                    else if (chkbox.Text.Contains("Other"))
                    {
                        calllog.Others = getCallerTypeControlListValue("txtOther", true, TextBox);
                    }
                }
            }



            // CONTINUE HERE....................................



            string chklstBox1Item = string.Empty;
            if (isRequiredForSelectedRole(CommonConstants.SelectedRole, lstBox1ItemRequiredRoles))
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



                    if (!(containsIgnoreCase(cmbBoxName, "skill")
                            || containsIgnoreCase(cmbBoxName, "transfer")
                            || containsIgnoreCase(cmbBoxName, "reason")
                            || containsIgnoreCase(cmbBoxName, "documentrequest")))
                    {
                        Control[] ctlList = findCallType("cb" + number, true);
                        CheckBox chkBox = ((CheckBox)ctlList[0]);
                        ComboBox cmb = (ComboBox)cn;
                        if (chkBox != null && chkBox.Checked == true)
                        {
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

            // Added for CSR or NBCSR call type removal
            if ((CommonConstants.SelectedRole == POSRoles.CSR || CommonConstants.SelectedRole == POSRoles.NBCSR)
                && lstcallType.Count == 0)
            {
                calllog.CallType = string.Empty;
            }

            // Set Call Log values
            calllog.CallType = buildValueForList(calllog.CallType, lstcallType);
            calllog.CallerType = buildValueForList(calllog.CallerType, lstClrType);
            calllog.CallerName = buildValueForList(calllog.CallerName, lstcallerName);
            calllog.CallType = buildValueForString(calllog.CallType, result1);
            calllog.CallType = buildValueForString(calllog.CallType, chklstBox1Item);
            calllog.CallType2 = buildValueForList(calllog.CallType2, lstcallType2);
            calllog.CallType3 = buildValueForList(calllog.CallType3, lstcallType3);
            calllog.CallType4 = buildValueForList(calllog.CallType4, lstcallType4);
            calllog.AdditionalSearchNos = getAdditionalSearchNosValue(lbxAdditionalSearch); //TODO follow up on type
            calllog.CallNotes = txtbxNotes.Text;
            calllog.TransferFrom = string.Empty;

            // Set Related Reference if role is of CSR
            if (CommonConstants.SelectedRole == POSRoles.CSR)
            {
                calllog.RelatedRef = varRelatedRef; //TODO Find out where this comes from
            }

            // If CallType is null
            if (calllog.CallType == null)
            {
                // And we meet these conditions
                if ((isNoCallerSelected &&
                        (CommonConstants.SelectedRole == POSRoles.CSR
                            || CommonConstants.SelectedRole == POSRoles.LTC
                            || CommonConstants.SelectedRole == POSRoles.SHRC
                            || CommonConstants.SelectedRole == POSRoles.PolicyMaintenance
                            || CommonConstants.SelectedRole == POSRoles.Distribution
                            || CommonConstants.SelectedRole == POSRoles.ADM
                            || CommonConstants.SelectedRole == POSRoles.IncomeBenefit
                            || CommonConstants.SelectedRole == POSRoles.CRR))
                    || ((CommonConstants.SelectedRole == POSRoles.LoansAndSurrender) && (isNoCallerSelected || isCallBackSelected)))
                {
                    // Then set the CallType value to an empty string
                    calllog.CallType = string.Empty;
                }
            }

            return calllog;

        }
        catch (Exception ex)
        {
            Logging.LogErrorMsg(this.Name, MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            throw new System.Exception("Exception in creating the CallLogUI object. ", ex);
        }
    }




    /********************************************************************
        Find Caller Details 
        Returns: Array of Control (Call Details) objects
    *********************************************************************/
    public Control[] findCallerDetails(string controlName, bool indicator)
    {
        return grpCallerDetails.Controls.Find(controlName, indicator);
    }


    /********************************************************************
        Find Call Types 
        Returns: Array of Control (Call Type) objects
    *********************************************************************/
    public Control[] findCallType(string controlName, bool indicator)
    {
        return gbCalltype.Controls.Find(controlName, indicator);
    }


    /********************************************************************
        Build string value for List
        Returns: string
    *********************************************************************/
    public string buildValueForList(string callTypeStr, List<string> callTypeList)
    {
        foreach (string str in callTypeList)
        {
            if (!string.IsNullOrWhiteSpace(callTypeStr))
                callTypeStr = callTypeStr + "," + str;
            else
                callTypeStr = str;
        }

        return callTypeStr;
    }


    /********************************************************************
        Build string value for string 
        Returns: string
    *********************************************************************/
    public string buildValueForString(string callTypeStr, string resultStr)
    {
        if (!string.IsNullOrEmpty(resultStr))
        {
            if (!string.IsNullOrWhiteSpace(callTypeStr))
            {
                callTypeStr = callTypeStr + "," + resultStr;
            }
            else
            {
                callTypeStr = resultStr;
            }
        }

        return callTypeStr;
    }


    /********************************************************************
        Build value for Additional Search  list
        Returns: string
    *********************************************************************/
    public string getAdditionalSearchNosValue(List<Items> lbxAdditionalSearch)
    {
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
        return stradd;
    }


    /********************************************************************
        Reset Call Types
        Returns: void
    *********************************************************************/
    public void resetCallTypes()
    {
        lstcallType1.Clear();
        lstcallType2.Clear();
        lstcallType3.Clear();
        lstcallType4.Clear();
        lstcallerName.Clear();
    }


    /********************************************************************
        Does Array have items
        Returns: boolean
    *********************************************************************/
    public bool hasRecords(Control[] controlArray)
    {
        if (controlArray.Count() > 0)
            return true;
        else
            return false;
    }


    /********************************************************************
        String Contains to handle case
        Returns: boolean
    *********************************************************************/
    public bool containsIgnoreCase(string value1, string value2)
    {
        return value1.ToLower().Contains(value2);
    }


    /********************************************************************
        Get value for a given Caller Details Control List
        Returns: string
    *********************************************************************/
    public string getCallerDetailsControlListValue(string controlName, bool indicator, Type clazz)
    {
        Control[] controlList = findCallerDetails(controlName, indicator);
        if (hasRecords(controlList))
            return ((clazz)controlList[0]).Text;
        else
            return string.Empty;
    }


    /********************************************************************
        Get value for a given Caller Details Control List
        Returns: string
    *********************************************************************/
    public string getCallerTypeControlListValue(string controlName, bool indicator, Type clazz)
    {
        Control[] controlList = findCallType(controlName, indicator);
        if (hasRecords(controlList))   // TODO Verify if .Count is the same as .Length
            return ((clazz)controlList[0]).Text;
        else
            return string.Empty;
    }



    /********************************************************************
        Get Value based on Null Check. Return empty string if null.
        Returns: string
    *********************************************************************/
    public string getValueWithNullCheck(string value)
    {
        if (!string.IsNullOrEmpty(value))
            return value;
        else
            return string.Empty;
    }


    /********************************************************************
        Is the selected role required for the Required List of Roles
        Returns: boolean
    *********************************************************************/
    public bool isRequiredForSelectedRole(string selectedRole, string[] lstRequiredRoles)
    {
        // Check selectedRole in the list of roles to see if value required
        foreach (string role in lstRequiredRoles)
        {
            if (selectedRole == role)
            {
                return true;
            }
        }
        return false;
    }

    /********************************************************************
        Is this for a General Question, No Caller Selected, or
        Call Back Selected?
        Returns: boolean
    *********************************************************************/
    public bool IsGeneralQuestOrNoCallerOrCallBack()
    {
        if (isGeneralQuestion || isNoCallerSelected || isCallBackSelected)
            return true;
        else
            return false;
    }


}
