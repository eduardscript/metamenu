"use client";

import { FC, useEffect, useRef } from "react";

const DeleteTagCategoryForm: FC<{ state: any; model?: any }> = ({
  state,
  model,
}) => {
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
      <input name="id" type="hidden" value={model.id} />
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <p className="py-4">
        Are you sure you want to delete tag category {model.id}?
      </p>
      <p className="font-bold text-sm italic">(This action cannot be undone)</p>
    </>
  );
};

export default DeleteTagCategoryForm;
