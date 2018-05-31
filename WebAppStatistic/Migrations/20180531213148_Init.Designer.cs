﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebAppPhotoSiteImages.Database;

namespace WebAppStatistic.Migrations
{
    [DbContext(typeof(DbMgmtStat))]
    [Migration("20180531213148_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Common.DataEntities.UserAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<DateTime>("DateCreated");

                    b.Property<Guid?>("EntityId");

                    b.Property<string>("UserInfo");

                    b.Property<Guid>("UserProfileId");

                    b.HasKey("Id");

                    b.ToTable("UserActions");
                });
#pragma warning restore 612, 618
        }
    }
}
