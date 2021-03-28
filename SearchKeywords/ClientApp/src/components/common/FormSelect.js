import React from "react";
import ErrorMessage from "./ErrorMessage";

const FormSelect = ({
  className,
  error,
  id,
  items,
  name,
  onChange,
  title,
  value,
}) => {
  return (
    <>
      <span className="mr-2">{title}</span>
      <select
        id={id}
        name={name}
        className={className}
        onChange={onChange}
        value={value}
      >
        <option>---Select---</option>
        {items.map((item, index) => (
          <option key={index} value={item.name}>
            {item.name}
          </option>
        ))}
      </select>
      <div className="text-danger">
        {error && <ErrorMessage error={error} />}
      </div>
    </>
  );
};

export default FormSelect;
