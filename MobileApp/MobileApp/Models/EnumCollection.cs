using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    class EnumCollection
    {
    }


    public enum TypeProfileView
    {
        ChangeProfile, ChangePassword, Absen, Pelanggaran, Pelaporan, Perusahaan, MutasiPerusahaan 
    }

    public enum StatusPelanggaran
    {
        Baru, Terima, Tolak
    }


    public enum AbsenType
    {
        None, Kerja, Lembur
    }

   
    public enum PelanggaranType
    {
        Pelanggaran, Pengaduan
    }


    public enum NotificationType
    {
        Public, Private
    }



}
