import clsx from 'clsx';
import { useState } from 'react';
import Select from 'react-select';

interface Tenant {
  code: number;
  name: string;
}
type SelectOptionType = { label: string, value: number }

interface TenantDropdownProps {
  tenants: Tenant[];
  onChange?: (selectedTenant: Tenant | null) => void;
  className?: string;
}

const TenantDropdown: React.FC<TenantDropdownProps> = ({ tenants, onChange, className }) => {
  const [selectedTenant, setSelectedTenant] = useState<Tenant | null>(null);

  const options = tenants.map((tenant) => ({
    value: tenant.code,
    label: `${tenant.code} - ${tenant.name}`,
  }));

  const handleChange = (selectedOption: SelectOptionType | null) => {
    const newSelectedTenant = selectedOption ? tenants.find((tenant) => tenant.code === selectedOption.value) || null : null;

    if (onChange) {
      onChange(newSelectedTenant);
    }

    setSelectedTenant(newSelectedTenant);
  };

  return (
    <Select
      options={options}
      value={selectedTenant ? { value: selectedTenant.code, label: selectedTenant.name } : null}
      onChange={handleChange}
      isSearchable
      placeholder="Please select a tenant"
      className={clsx("mb-2", className)}
    />
  );
};

export default TenantDropdown;
