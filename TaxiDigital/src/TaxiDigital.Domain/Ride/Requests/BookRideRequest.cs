namespace TaxiDigital.Domain.Ride.Requests;

public sealed class BookRideRequest
{
    public string user_email { get; set; }
    public string user_phone { get; set; }
    public string user_name { get; set; }
    public string booking_hash { get; set; }
    public string init_address_name { get; set; }
    public double init_address_lat { get; set; }
    public double init_address_lng { get; set; }
    public string init_address_number { get; set; }
    public string end_address_name { get; set; }
    public double end_address_lat { get; set; }
    public double end_address_lng { get; set; }
    public int payment_id { get; set; }
    public int category_id { get; set; }
    public string unique_field { get; set; }

    //Define se o serviço será agendado ou não
    //0 - Pede o serviço de momento
    //1 - Agenda o serviço. (neste caso os campos schedule_date e schedule_time são obrigatórios)
    public int schedule { get; set; }

    //Data do agendamento. Formato dd/MM/yyyy
    public string schedule_date { get; set; }

    //Horário do agendamento. Formato HH:mm
    public string schedule_time { get; set; }

    //Tempo em minutos de antecedência em que o serviço agendado será disponíbilizado para os motoristas
    public int schedule_lead_time { get; set; }

    //Campos não obrigatórios
    public string init_reference { get; set; }
    public string init_cep { get; set; }
    public double init_real_lat { get; set; }
    public double init_real_lng { get; set; }
    public string external_authorization_id { get; set; }
    public string estimate_fare { get; set; }
    public string estimate_km { get; set; }
    public string user_imei { get; set; }
    public string user_platform { get; set; }
    public string share_group { get; set; }
    public string contract_number { get; set; }
    public string estimate_id { get; set; }
    public string preferences { get; set; }
    public List<UniqueFieldRequest> unique_fields { get; set; }

}
