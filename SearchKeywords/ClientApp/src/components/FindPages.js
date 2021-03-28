import React, { useState } from "react";
import FormInput from "./common/FormInput";
import Input from "./common/Input";
import Loading from "./common/Loading";

function FindPages() {
  const [loading, setLoading] = useState(false);
  const [pages, setPages] = useState([]);

  const [errors, setErrors] = useState({
    keywords: "",
    url: "",
  });

  const [params, setParams] = useState({
    keywords: "online title search",
    url: "https://www.infotrack.com.au",
  });

  const handleChange = ({ currentTarget: input }) => {
    setParams((prevData) => {
      return { ...prevData, [input.name]: input.value };
    });

    // clear error messages
    setErrors((prevErrors) => {
      return { ...prevErrors, [input.name]: "" };
    });
  };

  const setErrorMessage = () => {
    let validated = true;

    if (params.keywords === "") {
      setErrors((prevErrors) => {
        return { ...prevErrors, keywords: "Keywords is required." };
      });
      validated = false;
    }

    if (params.url === "") {
      setErrors((prevErrors) => {
        return { ...prevErrors, url: "Url is required." };
      });
      validated = false;
    }
    return validated;
  };

  const validateForm = () => {
    return setErrorMessage();
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!validateForm()) {
      return false;
    }

    fetchPages();
  };

  const fetchPages = async () => {
    try {
      setLoading(true);

      const endPoint = `SearchKeywords/${params.keywords}/${encodeURIComponent(params.url)}`;

      const response = await fetch(endPoint);
      const data = await response.json();
      setPages(data);

      setLoading(false);
    } catch (error) {
      setLoading(false);
      console.log("unable to get pages", error);
    }
  };

  if (loading)
    return (
      <div className="loading">
        <Loading />
      </div>
    );

  return (
    <div className="container m-5">
      <div className="form-container">
        <form className="form-inline" onSubmit={handleSubmit}>
          <div className="form-group mb-2">
            <div>
              <FormInput
                className="form-control mr-3"
                error={errors.keywords}
                id="keywords"
                name="keywords"
                title="Key Words:"
                type="text"
                value={params.keywords}
                onChange={handleChange}
              />
            </div>
            <div>
              <FormInput
                className="form-input-width form-control mr-3"
                error={errors.url}
                id="url"
                name="url"
                title="Url:"
                type="text"
                value={params.url}
                onChange={handleChange}
              />
            </div>
            <Input
              className="btn btn-primary btn-sm ml-2"
              type="submit"
              value="Search"
            />
          </div>
        </form>
      </div>
      <div className="m-3">
        <table className="table table-striped">
          <thead>
            <tr>
              <th scope="col">Search Engine</th>
              <th scope="col">Pages</th>
              <th scope="col">Keywords</th>
              <th scope="col">Url</th>
            </tr>
          </thead>
          <tbody>
            {pages.map((item, index) => {
              return (
                <tr key={index}>
                  <td>{item.name}</td>
                  <td>{item.pages}</td>
                  <td>{item.keywords}</td>
                  <td>{item.url}</td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default FindPages;
