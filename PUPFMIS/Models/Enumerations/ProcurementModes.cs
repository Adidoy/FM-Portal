using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public enum ProcurementModes
    {
        /*
        Competitive Bidding
        Limited Source Bidding
        Direct Contracting
        Repeat Order
        Shopping
        NP-53.1 Two Failed Biddings
        NP-53.2 Emergency Cases
        NP-53.3 Take-Over of Contracts
        NP-53.4 Adjacent or Contiguous
        NP-53.5 Agency-to-Agency
        NP-53.6 Scientific, Scholarly, Artistic Work, Exclusive Technology and Media Services
        NP-53.7 Highly Technical Consultants
        NP-53.8 Defense Cooperation Agreement
        NP-53.9 - Small Value Procurement
        NP-53.10 Lease of Real Property and Venue
        NP-53.11 NGO Participation
        NP-53.12 Community Participation
        NP-53.13 UN Agencies, Int'l Organizations or International Financing Institutions
        Others - Foreign-funded procurement
        */

        [Display(Name = "Competitive Bidding")]
        PBID = 0,
        [Display(Name = "Limited Source Bidding")]
        LSBD = 1,
        [Display(Name = "Direct Contracting")]
        DICO = 2,
        [Display(Name = "Repeat Order")]
        REOR = 3,
        [Display(Name = "Shopping")]
        SHOP = 4,
        [Display(Name = "NP-53.1 Two Failed Biddings")]
        TFBD = 5,
        [Display(Name = "NP-53.2 Emergency Cases")]
        EMCA = 6,
        [Display(Name = "NP-53.3 Take-over of Contracts")]
        TOOC = 7,
        [Display(Name = "NP-53.4 Adjacent or Contiguous")]
        AJCO = 8,
        [Display(Name = "NP-53.6 Scientific, Scholarly, Artistic Work, Exclusive Technology and Media Services")]
        EXCL = 9,
        [Display(Name = "NP-53.7 Highly Technical Consultants")]
        HTCO = 10,
        [Display(Name = "NP-53.8 Defense Cooperation Agreement")]
        DCAG = 11,
        [Display(Name = "NP-53.9 Small Value Procurement")]
        SVPR = 12,
        [Display(Name = "NP-53.10 Lease of Real Property or Venue")]
        LRPV = 13,
        [Display(Name = "NP-53.11 NGO Participation")]
        NGOP = 14,
        [Display(Name = "NP-53.12 Community Participation")]
        COPA = 15,
        [Display(Name = "NP-53.13 UN Agencies, Int'l Organizations or International Financing Institutions")]
        INTL = 16,
        [Display(Name = "Others - Foreign-funded procurement")]
        FFPR = 17
    }
}