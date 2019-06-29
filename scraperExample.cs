using System;
using System.Data;
using System.IO;
using System.Text;

namespace SDATScrape
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Input file path containing column of reference data used in scrape, access database .mdb format is used.
            string inFile = null;
            if (args.Length == 0)
            {
                Console.Write("Please enter the desired file path: ");
                inFile = "C:\\\\Temp\\\\savedOutputs\\\\SWOData.mdb";
                inFile = inFile.Substring(0, inFile.Length);
            }

            string outFile = inFile.Substring(0, inFile.LastIndexOf('.')) + ".psv";
            Console.WriteLine("Scraping File " + inFile + "...");

            try
            {
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();

                conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + inFile + ";Persist Security Info=False";
                conn.Open();

                String sql = "SELECT [Parcels.parcel number] FROM Parcels;";
                System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                System.Data.OleDb.OleDbDataReader oRead = cmd.ExecuteReader();

                DataTable table1 = new DataTable();
                table1.Load(oRead);

                string tempString = null;
                int count = 0;
                byte[] buf = new byte[8192];
                int r = 1;
                int c = table1.Rows.Count;

                StreamWriter tWrite = new StreamWriter(outFile, false);
                tWrite.AutoFlush = true;
                tWrite.WriteLine("PIN|mailingAddress");

                foreach (DataRow row in table1.Rows)
                {
                    if (r % 1000 == 0)
                    {
                        Console.WriteLine(r.ToString() + " records...");
                    }

                    //Build URL and request, attach scrape references to end of URL
                    string url = "http://www.SiteToScrapeFrom?ParcelID=" + (string) row[0] + "%20%20";
                    StringBuilder sb = new StringBuilder();

                    if (url != null)
                    {
                        System.Net.HttpWebRequest hwr = (System.Net.HttpWebRequest) System.Net.WebRequest.Create(url);

                        System.Net.HttpWebResponse hresp = (System.Net.HttpWebResponse) hwr.GetResponse();
                        System.IO.Stream resStream = hresp.GetResponseStream();

                        tempString = null;
                        count = 0;

                        do
                        {
                            count = resStream.Read(buf, 0, buf.Length);
                            if (count != 0)
                            {
                                tempString = Encoding.ASCII.GetString(buf, 0, count);
                                sb.Append(tempString);
                            }
                        } while (count > 0);
                    }

                    //Parse response for data
                    string pageString = sb.ToString();
                    string mailingaddress = "0";

                    if (!pageString.Contains("There are no records that match your criteria"))
                    {
                        //Check page html for desired data ID, copy data stored within that ID.
                        pageString = pageString.Substring(pageString.IndexOf("lblChangeMail"));

                        pageString = pageString.Substring(pageString.IndexOf(">") + 1);
                        try { mailingaddress = pageString.Substring(0, pageString.IndexOf("</")); }
                        catch { }
                    }

                    tWrite.WriteLine((string) row[0] + "|" + mailingaddress);
                    r++;
                }
                conn.Close();
                tWrite.Close();
                tWrite.Dispose();

                Console.WriteLine("Scrape Complete!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            finally
            {
            }
        }
    }
}