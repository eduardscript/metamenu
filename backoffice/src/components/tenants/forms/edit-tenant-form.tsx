"use client";

import { FC, useEffect, useRef } from "react";

const EditTenantForm: FC<{ state: any; model?: any }> = ({ state, model }) => {
  const inputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    if (inputRef.current) {
      const inputElement = inputRef.current;
      inputElement.focus();
      const valueLength = inputElement.value.length;
      inputElement.setSelectionRange(valueLength, valueLength);
    }
  }, []);

  return (
    <>
      <input name="id" type="hidden" value={model.code} />
      <p className="py-4">
        <input
          ref={inputRef}
          name="name"
          className="input input-bordered"
          defaultValue={model.name}
        />
        {state.errors?.name && (
          <span className="text-red-500 block">{state.errors.name}</span>
        )}
      </p>
      <p className="py-4">
        <input
          name="isActive"
          type="checkbox"
          className="toggle toggle-success"
          defaultChecked={model.isActive}
        />
      </p>
    </>
  );
};

export default EditTenantForm;
