﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LYtest.BaseBlocks;
using LYtest.CFG;
using LYtest.IterAlg;
using LYtest.LinearRepr.Values;

namespace LYtest.ReachingDefs
{

    public sealed class ReachingDefsIterAlg : IterativeCommonAlg<HashSet<LabelValue>>
    {
        protected override HashSet<LabelValue> Top => new HashSet<LabelValue>();

        private readonly GenKillBuilder GenKill;

        public ReachingDefsIterAlg(CFGraph g) : base(g)
        {
            GenKill = new GenKillBuilder(g.Blocks);
            Run();
        }

        protected override bool ContCond(HashSet<LabelValue> a, HashSet<LabelValue> b)
        {
            return !a.SetEquals(b);
        }

        protected override HashSet<LabelValue> TransferFunc(IBaseBlock b)
        {
            var res = GenKill.GenLabels(b);
            res.UnionWith(In[b].Except(GenKill.KillLabels(b)));
            return res;
        }

        protected override HashSet<LabelValue> MeetOp(List<CFGNode> nodes)
        {
            return new HashSet<LabelValue>(nodes.SelectMany(n => Out[n.Value]));
        }
    }
}
