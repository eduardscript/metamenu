import React, { useState } from 'react';

const NewTagCategoryForm: React.FC<{
  onSubmit: (tagCategory: { code: string }) => void;
  onCancel: () => void;
}> = ({ onSubmit, onCancel }) => {
  const [code, setCode] = useState<string | undefined>();

  const handleSubmit = () => {
    if (code !== undefined) {
      onSubmit({ code });
    }
  };

  return (
    <div className="bg-white p-4">
      <h2 className="text-lg font-semibold mb-4">Create new tag category</h2>
      <label className="block mb-2">Code:</label>
      <input
        type="text"
        className="border w-full p-2 mb-4"
        onChange={(e) => setCode(e.target.value)}
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

export default NewTagCategoryForm;
