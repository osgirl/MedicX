﻿// MedicX
// Copyright (C) 2017-2018 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using DustInTheWind.MedicX.Persistence.Json.Entities;
using Newtonsoft.Json;

namespace DustInTheWind.MedicX.Persistence.Json
{

    /// <summary>
    /// Responsability: Reads and writes the 
    /// </summary>
    public class JsonZipFile
    {
        private readonly string fileName;

        public MedicXData Data { get; set; }

        public JsonZipFile(string fileName)
        {
            this.fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public void Open()
        {
            if (!File.Exists(fileName))
                Data = new MedicXData();

            MedicXData medicXData = new MedicXData();

            using (FileStream fileStream = File.OpenRead(fileName))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        switch (entry.Name)
                        {
                            case "medics.json":
                                medicXData.Medics = ReadFile<List<Medic>>(entry);
                                break;

                            case "clinics.json":
                                medicXData.Clinics = ReadFile<List<Clinic>>(entry);
                                break;

                            case "consultations.json":
                                medicXData.Consultations = ReadFile<List<Consultation>>(entry);
                                break;

                            case "investigations.json":
                                medicXData.Investigations = ReadFile<List<Investigation>>(entry);
                                break;

                            case "investigation-descriptions.json":
                                medicXData.InvestigationDescriptions = ReadFile<List<InvestigationDescription>>(entry);
                                break;
                        }
                    }
                }
            }

            Data = medicXData;
        }

        private static T ReadFile<T>(ZipArchiveEntry entry)
        {
            using (Stream stream = entry.Open())
            using (StreamReader streamReader = new StreamReader(stream))
            using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                return jsonSerializer.Deserialize<T>(jsonTextReader);
            }
        }

        public void Save()
        {
            using (FileStream fileStream = File.Create(fileName))
            {
                using (ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    AddFile(archive, Data.Medics, "medics.json");
                    AddFile(archive, Data.Clinics, "clinics.json");
                    AddFile(archive, Data.Consultations, "consultations.json");
                    AddFile(archive, Data.Investigations, "investigations.json");
                    AddFile(archive, Data.InvestigationDescriptions, "investigation-descriptions.json");
                }
            }
        }

        private static void AddFile(ZipArchive archive, object o, string fileName)
        {
            ZipArchiveEntry zipArchiveEntry = archive.CreateEntry(fileName);

            using (Stream stream = zipArchiveEntry.Open())
            using (StreamWriter streamWriter = new StreamWriter(stream))
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                jsonTextWriter.Formatting = Formatting.Indented;

                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                JsonSerializer jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);
                jsonSerializer.Serialize(jsonTextWriter, o);
            }
        }
    }
}