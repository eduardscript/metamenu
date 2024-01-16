import React, { useState } from 'react';

const NewTenantForm: React.FC<{
  onSubmit: (tenant: { name: string }) => void;
  onCancel: () => void;
}> = ({ onSubmit, onCancel }) => {
  const [name, setName] = useState<string | undefined>();

  const handleSubmit = () => {
    if (name !== undefined) {
      onSubmit({ name });
    }
  };

  return (
    <div className="bg-white p-4">
      <h2 className="text-lg font-semibold mb-4">Create New Tenant</h2>
      <label className="block mb-2">Name:</label>
      <input
        type="text"
        className="border w-full p-2 mb-4"
        onChange={(e) => setName(e.target.value)}
        autoFocus
      />
      <div className="flex justify-end">
        <button className="bg-blue-500 text-white px-4 py-2 rounded mr-2" onClick={handleSubmit}>
          Create
        </button>
        <button className="border px-4 py-2 rounded" onClick={onCancel}>
          Cancel
        </button>
      </div>
    </div>
  );
};

export default NewTenantForm;
