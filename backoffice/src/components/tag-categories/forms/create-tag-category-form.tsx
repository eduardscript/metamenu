"use client";

export default function CreateTagCategoryForm({
  state,
  model,
}: {
  state: {
    errors?: {
      code?: string[];
      _server?: string[];
    };
  };
  model?: any;
}) {
  return (
    <>
      <input type="hidden" name="tenantCode" value={model.tenantCode} />
      {state.errors?._server && (
        <span className="text-red-500 block">{state.errors._server}</span>
      )}
      <p className="py-4">
        <input
          name="code"
          className="input input-bordered"
          placeholder="tag category code"
        />
      </p>
      {state.errors?.code && (
        <span className="text-red-500 block">{state.errors.code}</span>
      )}
    </>
  );
}
