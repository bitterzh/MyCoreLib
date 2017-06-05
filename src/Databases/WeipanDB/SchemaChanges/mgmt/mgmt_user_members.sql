
CREATE TABLE IF NOT EXISTS mgmt_user_members(
	`user_id`               integer     NOT NULL COMMENT '��̨����Ա�ڲ����',
	`center_id`             integer     NOT NULL COMMENT '��Ӫ���ı��',
	`member_id`             integer     NOT NULL COMMENT '��Ա�ڲ����',
	`member_name`           varchar(64) NULL COMMENT '��Ա��½��',
	`parent_member_id`      integer     NULL COMMENT '�ϼ���Ա��ţ�0�����ۺϻ�Ա',
	`level`                 integer     NULL DEFAULT 0 COMMENT '�㼶',
	PRIMARY KEY (`user_id`,`center_id`, `member_id`)
)
AUTO_INCREMENT = 1000,
CHARACTER SET = 'utf8mb4';