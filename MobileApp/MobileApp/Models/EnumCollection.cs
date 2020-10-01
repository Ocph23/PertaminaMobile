using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp
{

    public enum Theme
    {
        White, Orange, Red
    }
    public enum TypeProfileView
    {
        ChangeProfile, ChangePassword, Absen, Pelanggaran, Pelaporan, Perusahaan, MutasiPerusahaan,
        Kejadian,
        Theme,
        About
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
