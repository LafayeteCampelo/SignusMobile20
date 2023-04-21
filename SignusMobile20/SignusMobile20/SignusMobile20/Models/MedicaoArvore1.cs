using SQLite;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SignusMobile20.Models
{
    public class MedicaoArvore1 : INotifyPropertyChanged, IEditableObject
    {

        public MedicaoArvore1()
        {


        }

        #region private variables


        private int _PKMedicaoArv1;
        private int _PKParcelasMedArv;
        private string _IdParcela;
        private int _ArvNum;
        private int _FusteNum;
        private string _TipoMedD;
        private float _HTotal;
        private float _MedD;
        private float _HDom;
        private float _HFuste;
        private float _DCopa;
        private float _HCopa;
        private string _Observacoes;
        private bool _Selec;

        #endregion

        #region Public Properties

        [PrimaryKey, AutoIncrement]
        public int PKMedicaoArv1
        {
            get { return _PKMedicaoArv1; }
            set
            {
                _PKMedicaoArv1 = value;
                RaisePropertyChanged("PKMedicaoArv1");
            }
        }

        public int PKParcelasMedArv
        {
            get { return _PKParcelasMedArv; }
            set
            {
                _PKParcelasMedArv = value;
                RaisePropertyChanged("PKParcelasMedArv");
            }
        }

        //public string IdParcela { get; set; }

        public string IdParcela
        {
            get { return _IdParcela; }
            set
            {
                _IdParcela = value;
                RaisePropertyChanged("IdParcela");
            }
        }

        public int CodMedicaoArv1SR { get; set; }

        public int CodMedicaoArv1MN { get; set; }
        public int ArvNum
        {
            get { return _ArvNum; }
            set
            {
                _ArvNum = value;
                RaisePropertyChanged("ArvNum");
            }
        }
        public int FusteNum
        {
            get { return _FusteNum; }
            set
            {
                _FusteNum = value;
                RaisePropertyChanged("FusteNum");
            }
        }
        public string TipoMedD
        {
            get { return _TipoMedD; }
            set
            {
                _TipoMedD = value;
                RaisePropertyChanged("TipoMedD");
            }
        }

        public double HTotal
        {
            get { return _HTotal; }
            set
            {
                _HTotal = (float)value;
                RaisePropertyChanged("HTotal");
            }
        }
        public double MedD
        {
            get { return _MedD; }
            set
            {
                _MedD = (float)value;
                RaisePropertyChanged("MedD");
            }
        }
        public double HDom
        {
            get { return _HDom; }
            set
            {
                _HDom = (float)value;
                RaisePropertyChanged("HDom");
            }
        }
        public double HFuste
        {
            get { return _HFuste; }
            set
            {
                _HFuste = (float)value;
                RaisePropertyChanged("HFuste");
            }
        }
        public double DCopa
        {
            get { return _DCopa; }
            set
            {
                _DCopa = (float)value;
                RaisePropertyChanged("DCopa");
            }
        }
        public double HCopa
        {
            get { return _HCopa; }
            set
            {
                _HCopa = (float)value;
                RaisePropertyChanged("HCopa");
            }
        }

        public string Observacoes
        {
            get { return _Observacoes; }
            set
            {
                _Observacoes = value;
                RaisePropertyChanged("Observacoes");
            }
        }

        public bool Selec
        {
            get { return _Selec; }
            set
            {
                _Selec = value;
                RaisePropertyChanged("Selec");
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string Name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(Name));
        }

        private Dictionary<string, object> storedValues;
        public void BeginEdit()
        {
            storedValues = this.BackUp();
        }

        public void CancelEdit()
        {
            if (storedValues == null)
                return;

            foreach (var item in this.storedValues)
            {
                var itemProperties = GetType().GetTypeInfo().DeclaredProperties;
                var pDesc = itemProperties.FirstOrDefault(p => p.Name == item.Key);
                if (pDesc != null)
                    pDesc.SetValue(this, item.Value);
            }
        }

        public void EndEdit()
        {
            if (storedValues != null)
            {
                storedValues.Clear();
                storedValues = null;
            }
            Debug.WriteLine("End Edit Called");
        }

        protected Dictionary<string, object> BackUp()
        {
            var dictionary = new Dictionary<string, object>();
            var itemProperties = GetType().GetTypeInfo().DeclaredProperties;
            foreach (var pDescriptor in itemProperties)
            {
                if (pDescriptor.CanWrite)
                    dictionary.Add(pDescriptor.Name, pDescriptor.GetValue(this));
            }
            return dictionary;
        }

        #endregion

        public MedicaoArvore1(int PKMedicaoArv1, int PKParcelasMedArv, string IdParcela, int ArvNum, int FusteNum, string TipoMedD,
            double HTotal, double MedD, double HDom, double HFuste, double DCopa, double HCopa, string Observacoes, bool Selec)
        {
            this.PKMedicaoArv1 = _PKMedicaoArv1;
            this.PKParcelasMedArv = _PKParcelasMedArv;
            this.IdParcela = _IdParcela;
            this.ArvNum = _ArvNum;
            this.FusteNum = _FusteNum;
            this.TipoMedD = _TipoMedD;
            this.HTotal = _HTotal;
            this.MedD = _MedD;
            this.HDom = _HDom;
            this.HFuste = _HFuste;
            this.DCopa = _DCopa;
            this.HCopa = _HCopa;
            this.Observacoes = _Observacoes;
            this.Selec = _Selec;
        }
    }
}
