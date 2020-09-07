using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace testWS
{
    [Serializable()]
    public class mensaje{
        public Sis_informador sis_informador { get; set; }
         [XmlElement("sis_alerta")]
        public List<Sis_alerta> sis_alerta { get; set; }

        public mensaje(Sis_informador sis_informador) {
            this.sis_informador=sis_informador;
            sis_alerta = new List<Sis_alerta>();
        }

        public mensaje()
        {
        }
    }

    [Serializable()]
    public class Sis_informador {
        public String id_sis_informador { get; set; }
        public String estado { get; set; }
        public String num_sistemas { get; set; }
        public String time_generacion { get; set; }

        public Sis_informador(string id_sis_informador, string estado, string num_sistemas, string time_generacion)
        {
            this.id_sis_informador = id_sis_informador;
            this.estado = estado;
            this.num_sistemas = num_sistemas;
            this.time_generacion = time_generacion;
        }

        public Sis_informador()
        {
        }
    }

   // [Serializable()]



    [Serializable()]
    public class Alerta
    {
        public String id_alerta { get; set; }
        public String desc_alerta { get; set; }
        public String timestamp { get; set; }

        public Alerta(string id_alerta, string desc_alerta, string timestamp)
        {
            this.id_alerta = id_alerta;
            this.desc_alerta = desc_alerta;
            this.timestamp = timestamp;
        }
        public Alerta()
        {
        }
    }

    [Serializable()]
    public class Sis_alerta
    {
        public String id_sis_alerta { get; set; }
        public String num_alertas { get; set; }
        [XmlElement("alerta")]
        public List<Alerta> listAlertas { get; set; }

        public Sis_alerta(string id_sis_alerta, string num_alertas)
        {
            this.id_sis_alerta = id_sis_alerta;
            this.num_alertas = num_alertas;

            listAlertas = new List<Alerta>();
        }

        public Sis_alerta()
        {
        }
    }

    public class RespPost
    {
        public String status { get; set; }
        public String messages { get; set; }
    }

    public class RespGet
    {
        public String id { get; set; }
        public String envio { get; set; }
    }



    /****************************************************************/
    /****************************************************************/

    public class SenialConfig
    {
        public int IdUC;
        private String _sHexIdUC = "";
        public String sHexIdUC
        {
            get { return (_sHexIdUC); }
            set { _sHexIdUC = value; IdUC = Convert.ToInt32(_sHexIdUC, 16); }
        }

        public string NombreSenial { get; set; }
        public bool Activa { get; set; }
        public int getActivaEICs()
        {
            return (IdUC >> 8);
        }

        public ushort numBanda { get; set; }
    }


    [Serializable()]
    public class ConfigDataENCE
    {
        private List<Alerta> _listSeniales = new List<Alerta> { new Alerta {id_alerta="true",desc_alerta="0001", timestamp="E1" },
        };
        public List<Alerta> listSeniales { get; set; }

        public void ini()
        {
            listSeniales = new List<Alerta>();
            listSeniales.AddRange(_listSeniales);
        }
    }

    [XmlRoot("root")]
    public class RootTest
    {
        //[XmlArray(" "), XmlArrayItem("Alerta")]
        [XmlElement("Alerta")]
        public Alerta[] strings { get; set; }

        public String prueba = "kk";

        public RootTest()
        {
            this.strings=new Alerta[2];
        }
 
            
    }

    //string json = @"{  'FirstName': 'Lorenzo',    'LastName': 'Pardo'  }";
    //MyDetail bsObj = JsonConvert.DeserializeObject<MyDetail>(json);



    public class MyDetail
    {
        public string FirstName
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }
    }
  
 
}

//<mensaje> <sis_informador> <id_sis_informador>03107</id_sis_informador> <estado>OK</estado><num_sistemas>1</num_sistemas><time_generacion>2019_01_18 11_53_55</time_generacion></sis_informador><sis_alerta><id_sis_alerta>BAAMONDE</id_sis_alerta><num_alertas>1</num_alertas><alerta><id_alerta>51</id_alerta><desc_alerta>Eth1 conexion eliminada</desc_alerta><timestamp>2019_01_21 16_43_34</timestamp></alerta></sis_alerta></mensaje>
