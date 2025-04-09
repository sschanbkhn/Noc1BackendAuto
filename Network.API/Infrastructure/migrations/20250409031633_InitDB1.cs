using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Network.API.Infrastructure.migrations
{
    public partial class InitDB1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Net_AlarmType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_AlarmType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_CableManagement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CableCode = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    LineId = table.Column<Guid>(type: "uuid", nullable: true),
                    CableType = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    HeadDeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastDeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    SetPoint = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    ManageOrgan = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ManagerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ManagerTel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ManagerEmail = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_CableManagement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_ConfigurationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    BeforeConfig = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    AfterConfig = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_ConfigurationLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_CurenAlarm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlarmTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    LevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<Guid>(type: "uuid", nullable: true),
                    IncidentTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AlarmDetail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AlarmCode = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_CurenAlarm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_DevicePorts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    SerialPort = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PortFormat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Type = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    MaxSpeed = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_DevicePorts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Lon = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Lat = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DeviceTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: true),
                    FirmwareVersion = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    IPAddress = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MACAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SerialNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    OrganId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_DeviceTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_DeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_HistoryCurenAlarm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AlarmTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    LevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<Guid>(type: "uuid", nullable: true),
                    IncidentTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AlarmDetail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AlarmCode = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Reason = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ProcessedContent = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_HistoryCurenAlarm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_Manufacturers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Nation = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Net_NetworkLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SerialNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Distance = table.Column<int>(type: "integer", nullable: false),
                    HeadDeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastDeviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    HeadDevicePortId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastDevicePortId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConnectType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Speed = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Net_NetworkLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Authtokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    AccessToken = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RefeshToken = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Authtokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Configs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Value = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Configs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Path = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ObjectId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ObjectType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Content = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Receiver = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    DetailsURL = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ObjectId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ObjectType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Name = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsFunc = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Name = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Url = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Icon = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Name = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UserName = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    PassWord = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Email = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys_Users_Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Users_Roles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Net_AlarmType");

            migrationBuilder.DropTable(
                name: "Net_CableManagement");

            migrationBuilder.DropTable(
                name: "Net_ConfigurationLogs");

            migrationBuilder.DropTable(
                name: "Net_CurenAlarm");

            migrationBuilder.DropTable(
                name: "Net_DevicePorts");

            migrationBuilder.DropTable(
                name: "Net_Devices");

            migrationBuilder.DropTable(
                name: "Net_DeviceTypes");

            migrationBuilder.DropTable(
                name: "Net_HistoryCurenAlarm");

            migrationBuilder.DropTable(
                name: "Net_Manufacturers");

            migrationBuilder.DropTable(
                name: "Net_NetworkLinks");

            migrationBuilder.DropTable(
                name: "Sys_Authtokens");

            migrationBuilder.DropTable(
                name: "Sys_Categories");

            migrationBuilder.DropTable(
                name: "Sys_Configs");

            migrationBuilder.DropTable(
                name: "Sys_Files");

            migrationBuilder.DropTable(
                name: "Sys_Notification");

            migrationBuilder.DropTable(
                name: "Sys_Organizations");

            migrationBuilder.DropTable(
                name: "Sys_Permissions");

            migrationBuilder.DropTable(
                name: "Sys_Resources");

            migrationBuilder.DropTable(
                name: "Sys_Roles");

            migrationBuilder.DropTable(
                name: "Sys_Users");

            migrationBuilder.DropTable(
                name: "Sys_Users_Roles");
        }
    }
}
