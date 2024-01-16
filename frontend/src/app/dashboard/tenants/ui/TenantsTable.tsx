import React, { use, useEffect, useState } from 'react';
import Modal from '@/app/ui/Modal';
import { Input } from '@/app/ui/input';
import NewTenantForm from './NewTenantForm';

type Tenant = {
  code: number;
  name: string;
  createdAt: Date;
  isEnabled: boolean;
};

type TenantsTableProps = {
  tenants: Tenant[];
  onDelete: (code: number) => void;
  onDeactivate: (code: number, isEnabled: boolean) => void;
  onCreate: (tenant: { name: string }) => void;
};

const BUTTON_STYLE = 'px-2 py-2 rounded text-white cursor-pointer mr-2';
const DELETE_BUTTON_STYLE = 'bg-red-500 hover:bg-red-300';
const DEACTIVATE_BUTTON_STYLE = 'bg-blue-500 hover:bg-blue-300';
const TABLE_HEADER_STYLE = 'px-6 py-3 text-white bg-black text-left text-xs font-medium text-gray-500 uppercase tracking w-1/5';


const dateOptions: Intl.DateTimeFormatOptions = {
  day: '2-digit',
  month: '2-digit',
  year: 'numeric',
  hour: '2-digit',
  minute: '2-digit',
  second: '2-digit'
};

const TenantsTable: React.FC<TenantsTableProps> = ({ tenants, onDeactivate, onDelete, onCreate }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isOperating, setIsOperating] = useState<number | null>(null);
  const [filterText, setFilterText] = useState<string>('');
  const [currentPage, setCurrentPage] = useState<number>(1);

  const pageSize = 5;
  
  useEffect(() => {
    setCurrentPage(1);
  }, [filterText]);

  const handleDelete = (code: number) => {
    onDelete(code);
    setIsOperating(code);
  };

  const handleDeactivate = (code: number, isEnabled: boolean) => {
    onDeactivate(code, isEnabled);
    setIsOperating(code);
  };

  const handleCreateTenant = (tenant: { name: string }) => {
    onCreate(tenant);
    setIsModalOpen(false);
  };

  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const renderActionButtons = (tenant: Tenant) => (
    <>
      <button
        className={`${BUTTON_STYLE} ${DEACTIVATE_BUTTON_STYLE}`}
        onClick={() => handleDeactivate(tenant.code, !tenant.isEnabled)}
      >
        Deactivate
      </button>
      <button
        className={`${BUTTON_STYLE} ${DELETE_BUTTON_STYLE}`}
        onClick={() => handleDelete(tenant.code)}
      >
        Delete
      </button>
    </>
  );

  const renderTableHeader = () => (
    <thead>
      <tr>
        {['Code', 'Name', 'Created At', 'Enabled', 'Actions'].map((header) => (
          <th key={header} className={TABLE_HEADER_STYLE}>
            {header}
          </th>
        ))}
      </tr>
    </thead>
  );

  const renderTableBody = () => (
    <tbody>
      {getPaginatedTenants().map((tenant) => (
        <tr key={tenant.code}>
          <td className="px-6 py-4 whitespace-nowrap font-medium">{tenant.code}</td>
          <td className="px-6 py-4 whitespace-nowrap">{tenant.name}</td>
          <td className="px-6 py-4 whitespace-nowrap">{new Intl.DateTimeFormat('en-US', dateOptions).format(new Date(tenant.createdAt))}</td>
          <td className="px-6 py-4 whitespace-nowrap">
            <span className={`bg-black text-white px-2 py-2 rounded ${tenant.isEnabled ? 'bg-green-500' : 'bg-black'}`}>
              {tenant.isEnabled ? 'Yes' : 'No'}
            </span>
          </td>
          <td className="px-6 py-4 whitespace-nowrap">{renderActionButtons(tenant)}</td>
        </tr>
      ))}
    </tbody>
  );

  const getPaginatedTenants = () => {
    const startIndex = (currentPage - 1) * pageSize;
    const endIndex = startIndex + pageSize;
    return filteredTenants.slice(startIndex, endIndex);
  };

  const filteredTenants = tenants.filter((tenant) => {
    const filterTextAsNumber = Number(filterText);

    return (
      (!isNaN(filterTextAsNumber) && tenant.code.toString().includes(filterText)) ||
      tenant.name.toLowerCase().includes(filterText.toLowerCase())
    );
  });

  return (
    <>
      <div className="mb-4 flex flex-col">
        <Input
          placeholder="1000, Tasca do Spezas"
          label="Filter by name or code"
          onChange={(e) => setFilterText(e.target.value)}
        />
      </div>

      <div>
        <button
          className={`${BUTTON_STYLE} bg-black hover:bg-green-300 mb-4`}
          onClick={() => setIsModalOpen(true)}
        >
          Create new tenant
        </button>
        {isModalOpen && (
          <Modal onClose={() => setIsModalOpen(false)}>
            <NewTenantForm onSubmit={handleCreateTenant} onCancel={handleCancel} />
          </Modal>
        )}
      </div>

      <table className="min-w-full divide-y divide-gray-200 overflow-hidden rounded">
        {renderTableHeader()}
        {renderTableBody()}
      </table>

      {/* Pagination Controls */}
      <div className="mt-4">
        <button
          className="bg-blue-500 hover:bg-blue-300 px-2 py-1 rounded text-white mr-2"
          onClick={() => setCurrentPage(currentPage - 1)}
          disabled={currentPage === 1}
        >
          Previous
        </button>
        <span className="text-gray-700">
          Page {currentPage} of {Math.ceil(filteredTenants.length / pageSize)}
        </span>
        <button
          className="bg-blue-500 hover:bg-blue-300 px-2 py-1 rounded text-white ml-2"
          onClick={() => setCurrentPage(currentPage + 1)}
          disabled={currentPage === Math.ceil(filteredTenants.length / pageSize)}
        >
          Next
        </button>
      </div>
    </>
  );
};

export default TenantsTable;
