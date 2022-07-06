using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : FieldFunctionDefault
{
    public override void Function()
    {
        LoadNSave.Instance.Save();
    }

}
