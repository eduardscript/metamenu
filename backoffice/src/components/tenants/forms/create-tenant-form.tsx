"use client";

export default function CreateTenantForm({
  state,
}: {
  state: {
    errors?: {
      name?: string[];
      _form?: string[];
    };
  };
}) {
  return (
    <>
      <input
        name="name"
        className="input input-bordered"
        placeholder="tenant name"
      />
      {state.errors?.name && (
        <span className="text-red-500 block">{state.errors.name}</span>
      )}
    </>
  );
}
