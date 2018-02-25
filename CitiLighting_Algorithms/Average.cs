using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiLighting_Algorithms
{
    public class ConditionChangeEventArgs : PropertyChangedEventArgs{

        public CONDITION NewCondition { get; set; }
        public ConditionChangeEventArgs(string propertyName, CONDITION cond)
        : base(propertyName)
        {
            NewCondition = cond;
        }

    }
    public class Average : INotifyPropertyChanged
    {
        private decimal longaverage = 0.0M;
        private decimal shortaverage = 0.0M;
        public CONDITION Condition { get; set; }

        public Average(decimal longavg, decimal shortavg)
        {
            longaverage = longavg;
            shortaverage = shortavg;
            
        }
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }
        public bool ShortUnderLong()
        {
            if (ShortAverage < LongAverage)
            {

                condition = CONDITION.SHORTBELOWLONG;
                return true;
            }
            else
            {
                condition = CONDITION.LONGBELOWSHORT;
                return false;
            }

        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new ConditionChangeEventArgs(propertyName,Condition));
        }
        public decimal LongAverage
        {
            get { return longaverage; }
            set
            {
                longaverage = value;
                ShortUnderLong();
            }
        }

        public decimal ShortAverage
        {
            get { return shortaverage; }
            set
            {
                shortaverage = value;
                ShortUnderLong();
            }
        }
        public CONDITION condition
        {
            get { return Condition; }
            set
            {
                if (Condition == CONDITION.NONE)
                {
                    Condition = value;
                }
                else if (value != Condition)
                {
                    Condition = value;

                    OnPropertyChanged("condition");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
