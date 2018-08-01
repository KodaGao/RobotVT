/*==============================================================*/
/* Table: S_D_CameraSet                   */
/*==============================================================*/
create table S_D_CameraSet(
   VT_ID varchar(10)           default '' not null ,
   VT_NAME varchar(255)          default '' not null ,
   VT_PASSWORD varchar(40)           default '' not null ,
   VT_IP varchar(255)          default '' not null ,
   VT_PORT varchar(20)            default '' not null ,
   CONSTRAINT S_D_CameraSet PRIMARY KEY(VT_ID)
)

INSERT INTO S_D_CAMERASET (VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT) VALUES ('CLOUD','admin','zx123456','192.168.1.64','8000');
INSERT INTO S_D_CAMERASET (VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT) VALUES ('FRONT','admin','zx123456','192.168.1.65','8000');
INSERT INTO S_D_CAMERASET (VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT) VALUES ('BACK','admin','zx123456','192.168.1.66','8000');
INSERT INTO S_D_CAMERASET (VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT) VALUES ('LEFT','admin','zx123456','192.168.1.67','8000');
INSERT INTO S_D_CAMERASET (VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT) VALUES ('RIGHT','admin','zx123456','192.168.1.68','8000');
