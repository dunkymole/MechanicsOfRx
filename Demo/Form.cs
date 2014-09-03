using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalkNotes;

namespace Demo
{
    public partial class Form : System.Windows.Forms.Form
    {
        private IDisposable _disposable = Disposable.Empty;

        public Form()
        {
            InitializeComponent();

            SetupIdleWatch();
            SetupCalculate();
        }

        #region calculation

        private void SetupCalculate()
        {
            Go.Click += (sender, args) =>
                {
                    NaugtyNumbers.Items.Clear();
                    DoCalculation();
                };
            Stop.Click += (sender, args) => _disposable.Dispose();
        }

        private void DoCalculation()
        {
            var workers = Enumerable.Range(0, (int)WordCount.Value).Select(GetCounts);

            var q = from count in workers.Merge((int) Concurrency.Value)
                    where count != 4
                    select count;

            var sw = Stopwatch.StartNew();

            _disposable = q.ObserveOn(this).Subscribe(v => NaugtyNumbers.Items.Add(v), 
                                        e => MessageBox.Show(e.Message),
                                        () => MessageBox.Show(string.Format("Done in {0} seconds", sw.Elapsed.TotalSeconds)));
        }

        private static IObservable3<int> GetCounts(int number)
        {
            return Observable3.Create<int>(o =>
                {
                    var cts = new CancellationTokenSource();
                    Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                o.OnNext(LawOfFour.GetMagicNumber(number));
                                o.OnComplete();
                            }
                            catch (Exception e)
                            {
                                o.OnError(e);
                            }
                        }, cts.Token);
                    return Disposable.Create(cts.Cancel);
                });
        }

        #endregion

        #region idle

        private void SetupIdleWatch()
        {
            var timeouts = from move in MouseMoves
                           select Observable3Ex.Return(move).Concat(Observable3.Interval(TimeSpan.FromSeconds(3)).Take(1).Select(_=>false));
            
            timeouts.Switch().ObserveOn(this).Subscribe(move =>
                {
                    BackColor = move ? Color.Green : Color.Red;
                }, e => { }, () => { });
        }

        private IObservable3<bool> MouseMoves 
        {
            get
            {
                return Observable3.Create<bool>(o =>
                {
                    MouseEventHandler h = (sender, args) => o.OnNext(true);
                    MouseMove += h;
                    return Disposable.Create(() => { MouseMove -= h; });
                }); 
            }
        }

        #endregion
    }
}
