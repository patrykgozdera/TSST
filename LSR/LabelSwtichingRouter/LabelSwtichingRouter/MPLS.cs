﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    [Serializable]
    class MPLS
    {
        public Stack<int> labelStack;


        public int GetLabelFromStack()
        {
            return labelStack.Pop();
        }

        public void PutLabelOnStack(int l)
        {
            labelStack.Push(l);
        }

    }
}
