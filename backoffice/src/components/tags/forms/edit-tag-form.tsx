"use client";

import { FC, useEffect, useRef } from "react";
import SelectTagCategory from "../select-tag-category";

const EditTagForm: FC<{ state: any; model?: any }> = ({ state, model }) => {
  const inputRef = useRef<HTMLInputElement>(null);

  const { tag, tagCategories } = model;

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
      <input name="code" type="hidden" value={tag.code} />
      <input name="tagCategoryId" type="hidden" value={tag.tagCategoryId} />
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <p className="py-4">
        <input
          name="name"
          className="input input-bordered"
          defaultValue={tag.name}
          placeholder={tag.name}
          ref={inputRef}
        />
      </p>
      {state.errors?.name && (
        <span className="text-red-500 block">{state.errors.name}</span>
      )}
      <p>
        <SelectTagCategory isQuery={false} tagCategories={tagCategories} />
      </p>
    </>
  );
};

export default EditTagForm;
