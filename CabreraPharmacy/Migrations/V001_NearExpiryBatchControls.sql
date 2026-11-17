CREATE TABLE IF NOT EXISTS pharmacy_feature_migrations (
	migration_version VARCHAR(32) NOT NULL,
	migration_name VARCHAR(150) NOT NULL,
	applied_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY (migration_version)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS pharmacy_sale_item_batches (
	allocation_id BIGINT NOT NULL AUTO_INCREMENT,
	sale_item_id BIGINT NOT NULL,
	batch_id INT NOT NULL,
	quantity INT NOT NULL,
	allocated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY (allocation_id),
	UNIQUE KEY uq_sale_item_batch (sale_item_id, batch_id),
	KEY idx_sale_item_batches_batch (batch_id),
	CONSTRAINT fk_sale_item_batches_item FOREIGN KEY (sale_item_id)
		REFERENCES pharmacy_sale_items (sale_item_id) ON DELETE RESTRICT,
	CONSTRAINT fk_sale_item_batches_batch FOREIGN KEY (batch_id)
		REFERENCES inventory_batch (batch_id) ON DELETE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS inventory_batch_quarantines (
	quarantine_id BIGINT NOT NULL AUTO_INCREMENT,
	batch_id INT NOT NULL,
	quantity INT NOT NULL,
	reason VARCHAR(500) NOT NULL,
	status VARCHAR(16) NOT NULL DEFAULT 'Active',
	created_by INT NOT NULL,
	created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	released_by INT NULL,
	released_at DATETIME NULL,
	release_reason VARCHAR(500) NULL,
	PRIMARY KEY (quarantine_id),
	KEY idx_batch_quarantines_status (batch_id, status),
	KEY idx_batch_quarantines_created (created_at),
	CONSTRAINT fk_batch_quarantines_batch FOREIGN KEY (batch_id)
		REFERENCES inventory_batch (batch_id) ON DELETE RESTRICT,
	CONSTRAINT fk_batch_quarantines_created_by FOREIGN KEY (created_by)
		REFERENCES users (user_id) ON DELETE RESTRICT,
	CONSTRAINT fk_batch_quarantines_released_by FOREIGN KEY (released_by)
		REFERENCES users (user_id) ON DELETE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS inventory_batch_actions (
	action_id BIGINT NOT NULL AUTO_INCREMENT,
	batch_id INT NOT NULL,
	action_type VARCHAR(24) NOT NULL,
	quantity INT NOT NULL,
	reason VARCHAR(500) NOT NULL,
	status VARCHAR(16) NOT NULL DEFAULT 'Pending',
	requested_by INT NOT NULL,
	requested_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	confirmed_by INT NULL,
	confirmed_at DATETIME NULL,
	confirmation_note VARCHAR(500) NULL,
	cancelled_by INT NULL,
	cancelled_at DATETIME NULL,
	cancellation_reason VARCHAR(500) NULL,
	PRIMARY KEY (action_id),
	KEY idx_batch_actions_status (batch_id, status),
	KEY idx_batch_actions_requested (requested_at),
	CONSTRAINT fk_batch_actions_batch FOREIGN KEY (batch_id)
		REFERENCES inventory_batch (batch_id) ON DELETE RESTRICT,
	CONSTRAINT fk_batch_actions_requested_by FOREIGN KEY (requested_by)
		REFERENCES users (user_id) ON DELETE RESTRICT,
	CONSTRAINT fk_batch_actions_confirmed_by FOREIGN KEY (confirmed_by)
		REFERENCES users (user_id) ON DELETE RESTRICT,
	CONSTRAINT fk_batch_actions_cancelled_by FOREIGN KEY (cancelled_by)
		REFERENCES users (user_id) ON DELETE RESTRICT
) ENGINE=InnoDB;

INSERT IGNORE INTO pharmacy_feature_migrations (migration_version, migration_name)
VALUES ('V001', 'Near-expiry batch controls and allocations');
